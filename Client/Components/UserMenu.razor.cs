using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyE.Client.Components
{
    public partial class UserMenu
    {
        [Inject] public SignOutSessionStateManager SignOutManager { get; set; }
        [Inject] public PublicHttpClient PublicClient { get; set; }
        [Inject] public AuthenticationStateProvider StateProvider { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }

        private async Task BeginSignOut(MouseEventArgs args)
        {
            await SignOutManager.SetSignOutState();
            var request = new HttpRequestMessage(HttpMethod.Post, "api/account/logout");
            var response = await PublicClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var authService = StateProvider as RemoteAuthenticationService<RemoteAuthenticationState, RemoteUserAccount, ApiAuthorizationProviderOptions>;
                var remoteAuthContext = new RemoteAuthenticationContext<RemoteAuthenticationState>()
                {
                    Url = string.Empty,
                    State = new RemoteAuthenticationState() { ReturnUrl = Navigation.Uri }
                };
                await authService?.SignOutAsync(remoteAuthContext);
            }
        }
    }
}
