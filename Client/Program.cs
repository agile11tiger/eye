using Blazored.LocalStorage;
using EyE.Client.Handlers;
using EyE.Client.Services;
using EyE.Shared.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EyE.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            var services = builder.Services;
            builder.Logging.SetMinimumLevel(LogLevel.Information);
            builder.RootComponents.Add<App>("#app");
            var serverUri = builder.Configuration["ServerUri"];
            builder.Services.AddAuthorizationCore();

            //if AccessTokenNotAvailableException: https://chrissainty.com/avoiding-accesstokennotavailableexception-when-using-blazor-webassembly-hosted-template-with-individual-user-accounts/
            //services
            //    .AddScoped<BaseAuthorizationMessageHandler>()
            //    .AddHttpClient("EyE.ServerAPI", client => client.BaseAddress = new Uri(serverUri))
            //    // Supply HttpClient instances that include access tokens when making requests to the server project
            //    .AddHttpMessageHandler<BaseAuthorizationMessageHandler>();
            services
                .AddScoped(sp =>
                {
                    var client = new ServerHttpClient(
                        new DefaultBrowserOptionsMessageHandler(
                            new HttpClientHandler())
                        {
                            DefaultBrowserRequestMode = BrowserRequestMode.Cors,
                        })
                    {
                        BaseAddress = new Uri(serverUri),
                    };
                    client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
                    return client;
                })
                .AddScoped(sp =>
                {
                    var client = new PublicHttpClient(
                        new DefaultBrowserOptionsMessageHandler(
                            new HttpClientHandler())
                        {
                            DefaultBrowserRequestMode = BrowserRequestMode.Cors,
                        });
                    return client;
                });
        ////https://docs.microsoft.com/ru-ru/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-5.0
        //        .AddApiAuthorization(options =>
        //        {
        //            options.ProviderOptions.ConfigurationEndpoint = serverUri + "_configuration/EyE.Client";
        //        });

            builder.Services.AddScoped<ServerAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<ServerAuthenticationStateProvider>());
            builder.Services
                //https://github.com/Blazored/LocalStorage
                .AddBlazoredLocalStorage()
                .AddScoped<UserChecker>()
                .AddSingleton(JsonHelper.SerializeOptions);

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru");

            await builder.Build().RunAsync();
        }
    }

    public class PublicHttpClient : HttpClient
    {
        public PublicHttpClient(HttpMessageHandler handler) : base(handler)
        {
        }
    }

    public class ServerHttpClient : HttpClient
    {
        public ServerHttpClient(HttpMessageHandler handler) : base(handler)
        {
        }
    }
}
