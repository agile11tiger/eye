using EyEServer.Constants;
using EyEServer.Controllers;
using EyEServer.Data;
using EyEServer.Middlewares;
using EyEServer.Services;
using EyEServer.Services.Email;
using EyEServer.Services.Identity;
using EyEServer.Services.RoleInitializer;
using MemoryLib.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IO;
using System.Text;

[assembly: ApiController]
namespace EyEServer;

public class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (!Directory.Exists(LoggingController.LogDirectory))
            Directory.CreateDirectory(LoggingController.LogDirectory);

        //if (env.IsProduction())
        //todo add secrets
        ConfigureIOptionsData(builder);
        ConfigureServices(builder);
        var app = builder.Build();
        ServiceProvider = app.Services;
        InitializeRoles(app);
        Configure(app);
        app.Run();
    }

    private static void ConfigureIOptionsData(WebApplicationBuilder builder)
    {
        var data = builder.Configuration.GetSection("PettyBotEmailData");
        builder.Services.Configure<PettyBotEmailData>(data);
        builder.Services.Configure<RoleInitializerTestData>(builder.Configuration.GetSection("RoleInitializerTestData"));
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedUICultures = options.SupportedCultures = new[]
            {
                new CultureInfo("ru-RU"),
                new CultureInfo("en-US"),
            };
        });
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services
            .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection))
            .AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddScoped<TokenService, TokenService>();

        var allowedOrigins = new[] { builder.Configuration.GetValue<string>("ClientUri"), builder.Configuration.GetValue<string>("ServerUri") };
        builder.Services
            //Cors для ASP.NET Core
            .AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy", builder => builder
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

        builder.Services.AddSingleton<IPasswordHasher<UserModel>, PasswordHasherCustom>();//should be above AddDefaultIdentity
        builder.Services.AddSingleton<IUserValidator<UserModel>, UsernameValidatorCustom>();
        builder.Services.AddSingleton<IPasswordValidator<UserModel>, PasswordValidatorCustom>();
        builder.Services.AddDefaultIdentity<UserModel>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedAccount = true;
            options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider; //shortens token
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider; //shortens token
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        builder.Services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                ValidAudience = jwtSettings.GetSection("validAudience").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value))
            };
        });

        #region comment for identityServer
        //// Cors для IdentityServer
        //.AddSingleton<ICorsPolicyService>((container) => {
        //    var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
        //    return new DefaultCorsPolicyService(logger) { AllowedOrigins = allowedOrigins };
        //})
        //.AddDefaultIdentity<User>(options =>
        //{
        //    options.SignIn.RequireConfirmedAccount = true;
        //    options.Password.RequireNonAlphanumeric = false;
        //    options.Password.RequireDigit = false;
        //    options.Password.RequireLowercase = false;
        //    options.Password.RequireUppercase = false;
        //    options.Password.RequiredLength = 0;
        //    options.Password.RequiredUniqueChars = 0;
        //})
        //.AddRoles<IdentityRole>()
        //.AddEntityFrameworkStores<ApplicationDbContext>();

        //https://docs.microsoft.com/ru-ru/aspnet/core/blazor/security/webassembly/hosted-with-identity-server?view=aspnetcore-3.1&tabs=visual-studio
        //https://habr.com/ru/post/461433/
        //https://www.scottbrady91.com/Identity-Server/Using-ECDSA-in-IdentityServer4
        //builder.Services
        //    .AddIdentityServer(options =>
        //    {
        //        options.Endpoints.EnableJwtRequestUri = true;
        //        options.Events.RaiseErrorEvents = true;
        //        options.Events.RaiseFailureEvents = true;
        //        options.Events.RaiseInformationEvents = true;
        //        options.Events.RaiseSuccessEvents = true;
        //    })
        //   //.AddSigningCredential(new ECDsaSecurityKey(ECDsa.Create(ECCurve.NamedCurves.nistP256)), IdentityServerConstants.ECDsaSigningAlgorithm.ES256)
        //   //.AddSigningCredential(new RsaSecurityKey(RSA.Create()), IdentityServerConstants.RsaSigningAlgorithm.RS256)
        //   .AddApiAuthorization<User, ApplicationDbContext>(options => {
        //       options.IdentityResources["openid"].UserClaims.Add("role");
        //       options.ApiResources.Single().UserClaims.Add("role");
        //   });

        // Need to do this as it maps "role" to ClaimTypes.Role and causes issues
        //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");
        //builder.Services
        //    .AddAuthentication()
        //    .AddIdentityServerJwt();
        #endregion
        builder.Services.AddControllersWithViews();
        builder.Services
            .AddSingleton<EmailService>()
            .AddSingleton<RoleInitializerService>()
            .AddSingleton(JsonHelper.SerializeOptions)
            .AddHttpClient(HttpClientNames.LOCAL_CLIENT, config => config.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ServerUri")));

    }

    private static void InitializeRoles(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var roleInitializer = scope.ServiceProvider.GetService<RoleInitializerService>();
            roleInitializer.InitializeAsync(scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>()).Wait();
            roleInitializer.AddAdminAsync(scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>()).Wait();
            roleInitializer.AddUserAsync(scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>()).Wait();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

    private static void Configure(WebApplication app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
        app.UseIpLogger();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/api/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseRequestLocalization();
        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles(); //wwwroot
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
            RequestPath = new PathString("/Resources")
        });
        app.UseRouting();
        app.UseCors("DefaultCorsPolicy");
        //app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        //app.MapFallbackToFile("index.html");
    }
}
