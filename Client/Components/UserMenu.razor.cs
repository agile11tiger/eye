using Blazored.LocalStorage;
using MemoryClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
namespace MemoryClient.Components;

public partial class UserMenu
{
    private string _nickName;
    [Inject] public NavigationManager Navigation { get; set; }
    [Inject] public ILocalStorageService LocalStorage { get; set; }
    [Inject] public ServerHttpClient ServerHttpClient { get; set; }
    [Inject] public ServerAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    protected async override Task OnInitializedAsync()
    {
        var userInfo = await LocalStorage.GetItemAsync<UserInfo>(nameof(UserInfo));
        _nickName = userInfo?.Nickname;
        await base.OnInitializedAsync();
    }

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
