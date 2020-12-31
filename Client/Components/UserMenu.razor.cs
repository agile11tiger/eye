using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyE.Client.Components
{
    public partial class UserMenu
    {
        [Inject] public SignOutSessionStateManager SignOutManager { get; set; }
        [Inject] public PublicHttpClient PublicClient { get; set; }
        [Inject] public AuthenticationStateProvider StateProvider { get; set; }

        private async Task BeginSignOut(MouseEventArgs args)
        {
            await SignOutManager.SetSignOutState();
            var request = new HttpRequestMessage(HttpMethod.Post, "api/account/logout");
            var response = await PublicClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                //SignOutAsync(blazor)   https://github.com/dotnet/aspnetcore/blob/648c15dbe90fb8c113f7c6b4adeb40d9e10494f6/src/Components/WebAssembly/WebAssembly.Authentication/src/Services/RemoteAuthenticationService.cs
                //SignOut(ts)   https://github.com/dotnet/aspnetcore/blob/648c15dbe90fb8c113f7c6b4adeb40d9e10494f6/src/Components/WebAssembly/WebAssembly.Authentication/src/Interop/AuthenticationService.ts
                var authService = StateProvider as RemoteAuthenticationService<RemoteAuthenticationState, RemoteUserAccount, ApiAuthorizationProviderOptions>;
                var remoteAuthContext = new RemoteAuthenticationContext<RemoteAuthenticationState>()
                {
                    Url = string.Empty,
                    State = new RemoteAuthenticationState() { ReturnUrl = string.Empty }
                };
                await authService?.SignOutAsync(remoteAuthContext);
            }
        }
    }
}
