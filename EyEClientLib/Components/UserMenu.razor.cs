using Blazored.LocalStorage;
using EyEClientLib.Handlers;
using Microsoft.AspNetCore.Components.Web;
namespace EyEClientLib.Components;

public partial class UserMenu
{
    private string _nickname;
    [Inject] public ServerHttpClient ServerHttpClient { get; set; }
    [Inject] public NavigationManager Navigation { get; set; }
    [Inject] public ILocalStorageService LocalStorage { get; set; }
    [Inject] public ServerAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    protected async override Task OnInitializedAsync()
    {
        var userInfo = await LocalStorage.GetItemAsync<UserInfo>(nameof(UserInfo));
        _nickname = userInfo?.Nickname;
        await base.OnInitializedAsync();
    }

    private async Task AuthorizationCompleteAsync()
    {
        var userInfo = await LocalStorage.GetItemAsync<UserInfo>(nameof(UserInfo));
        _nickname = userInfo?.Nickname;
        StateHasChanged();
    }

    private async Task Logout(MouseEventArgs args)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/account/logout");
        var response = await ServerHttpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
            await AuthenticationStateProvider.NotifyUserLogoutAsync();
    }
}
