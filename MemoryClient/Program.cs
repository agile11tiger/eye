using Blazored.LocalStorage;
using MemoryClient.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using EyEClientLib.Extensions;
using MudBlazor;
using EyEClientLib.Services;
namespace MemoryClient;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        var services = builder.Services;
        builder.Logging.SetMinimumLevel(LogLevel.Information);
        builder.RootComponents.Add<App>("#app");
        var serverUri = builder.Configuration["ServerUri"];

        //if AccessTokenNotAvailableException: https://chrissainty.com/avoiding-accesstokennotavailableexception-when-using-blazor-webassembly-hosted-template-with-individual-user-accounts/
        //services
        //    .AddScoped<BaseAuthorizationMessageHandler>()
        //    .AddHttpClient("EyEServerAPI", client => client.BaseAddress = new Uri(serverUri))
        //    // Supply HttpClient instances that include access tokens when making requests to the server project
        //    .AddHttpMessageHandler<BaseAuthorizationMessageHandler>();
        services
            .AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = true;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 5000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            })
            .AddMudServices()
            .AddEyEClientServices()
            .AddAuthorizationCore()
            .AddScoped<UserChecker>()
            .AddBlazoredLocalStorage() //https://github.com/Blazored/LocalStorage
            .AddTransient<HttpClientHandler>()
            .AddSingleton(JsonHelper.SerializeOptions) //todo change
            .AddTransient<DefaultBrowserOptionsMessageHandler>()
            .AddScoped(sp =>
            {
                var browserMessageHandler = new DefaultBrowserOptionsMessageHandler(
                    sp.GetService<ISnackbar>(),
                    sp.GetService<LoggerService>(), 
                    sp.GetService<HttpClientHandler>());
                var client = new ServerHttpClient(browserMessageHandler) { BaseAddress = new Uri(serverUri) };
                client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
                return client;
            })
            .AddScoped(sp =>
            {
                var browserMessageHandler = new DefaultBrowserOptionsMessageHandler(
                    sp.GetService<ISnackbar>(),
                    sp.GetService<LoggerService>(),
                    sp.GetService<HttpClientHandler>());
                var client = new PublicHttpClient(browserMessageHandler);
                return client;
            });
        ////https://docs.microsoft.com/ru-ru/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-5.0
        //        .AddApiAuthorization(options =>
        //        {
        //            options.ProviderOptions.ConfigurationEndpoint = serverUri + "_configuration/MemoryClient";
        //        });

        builder.Services.AddScoped<ServerAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<ServerAuthenticationStateProvider>());
        var host = builder.Build();
        await host.SetCultureFromStorageAsync();
        await host.RunAsync();
    }
}

public class PublicHttpClient(HttpMessageHandler handler) : HttpClient(handler) { }
