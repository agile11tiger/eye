using Blazored.LocalStorage;
using MemoryClient.Services;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http;
namespace MemoryClient.Components;

public partial class UserMenu
{
    private string _nickname;
    [Inject] public NavigationManager Navigation { get; set; }
    [Inject] public ILocalStorageService LocalStorage { get; set; }
    [Inject] public ServerHttpClient ServerHttpClient { get; set; }
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
