using EyE.Shared.Helpers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyE.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //if AccessTokenNotAvailableException: https://chrissainty.com/avoiding-accesstokennotavailableexception-when-using-blazor-webassembly-hosted-template-with-individual-user-accounts/
            builder.Services
                .AddHttpClient("EyE.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("EyE.ServerAPI"));
            builder.Services.AddScoped(sp => new PublicHttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //https://docs.microsoft.com/ru-ru/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-5.0
            builder.Services.AddApiAuthorization();
            builder.Services.AddSingleton(JsonHelper.SerializeOptions);
            //builder.Services.AddScoped<ServerAuthenticationStateProvider>();
            //builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<ServerAuthenticationStateProvider>());

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru");

            await builder.Build().RunAsync();
        }
    }

    public class PublicHttpClient : HttpClient
    {
    }
}
