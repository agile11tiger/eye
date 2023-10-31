using Blazored.LocalStorage;
using EyE.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http;
namespace EyE.Client.Components;

public partial class UserMenu
{
    [Inject] public NavigationManager Navigation { get; set; }
    [Inject] public ILocalStorageService LocalStorage { get; set; }
    [Inject] public ServerHttpClient ServerHttpClient { get; set; }
    [Inject] public ServerAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private async Task Logout(MouseEventArgs args)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/account/logout");
        var response = await ServerHttpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            await LocalStorage.RemoveItemAsync(nameof(UserInfo));
            AuthenticationStateProvider.NotifyUserLogout();
        }
    }
}
