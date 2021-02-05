using EyE.Server.Data;
using EyE.Server.Middlewares;
using EyE.Server.Services;
using EyE.Shared.Helpers;
using EyE.Shared.ViewModels.Identity;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Cryptography;

[assembly: ApiController]
namespace EyE.Server
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddConfiguration(configuration);

            if (env.IsProduction())
                builder.AddJsonFile("appsettings.Production.private.json");

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider => Configuration);
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection))
                .AddDatabaseDeveloperPageExceptionFilter();

            var allowedOrigins = new[] { Configuration.GetValue<string>("ClientUri"), Configuration.GetValue<string>("ServerUri") };
            services
                //Cors для ASP.NET Core
                .AddCors(options =>
                {
                    options.AddPolicy("DefaultCorsPolicy", builder => builder
                        .WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                })
                // Cors для IdentityServer
                .AddSingleton<ICorsPolicyService>((container) => {
                    var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
                    return new DefaultCorsPolicyService(logger) { AllowedOrigins = allowedOrigins };
                })
                .AddDefaultIdentity<User>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 0;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //https://docs.microsoft.com/ru-ru/aspnet/core/blazor/security/webassembly/hosted-with-identity-server?view=aspnetcore-3.1&tabs=visual-studio
            //https://habr.com/ru/post/461433/
            //https://www.scottbrady91.com/Identity-Server/Using-ECDSA-in-IdentityServer4
            services
                .AddIdentityServer(options =>
                {
                    options.Endpoints.EnableJwtRequestUri = true;
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
               //.AddSigningCredential(new ECDsaSecurityKey(ECDsa.Create(ECCurve.NamedCurves.nistP256)), IdentityServerConstants.ECDsaSigningAlgorithm.ES256)
               //.AddSigningCredential(new RsaSecurityKey(RSA.Create()), IdentityServerConstants.RsaSigningAlgorithm.RS256)
               .AddApiAuthorization<User, ApplicationDbContext>(options => {
                   options.IdentityResources["openid"].UserClaims.Add("role");
                   options.ApiResources.Single().UserClaims.Add("role");
               });

            // Need to do this as it maps "role" to ClaimTypes.Role and causes issues
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");
            services
                .AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services
                .AddSingleton(new EmailService())
                .AddSingleton(JsonHelper.SerializeOptions)
                .AddLocalization(options => options.ResourcesPath = "Resources")
                .AddHttpClient("localClient", config => config.BaseAddress = new Uri(Configuration.GetValue<string>("ServerUri")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseIpLogger();

            if (env.IsDevelopment())
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

            var supportedCultures = new[] { new CultureInfo("ru") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("DefaultCorsPolicy");
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
