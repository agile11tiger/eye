using Blazored.LocalStorage;
using MemoryClient.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
namespace MemoryClient.Services;

public class ServerAuthenticationStateProvider(ServerHttpClient _serverHttpClient, ILocalStorageService _localStorage)
    : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var userInfo = await _localStorage.GetItemAsync<UserInfo>();
        ClaimsIdentity claimsIdentity = null;

        if (!string.IsNullOrWhiteSpace(userInfo?.Token))
        {
            _serverHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", userInfo.Token);
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(userInfo.Token);
            claimsIdentity = new ClaimsIdentity(jwtToken.Claims, "jwtAuth");
        }

        return new AuthenticationState(new ClaimsPrincipal(claimsIdentity ?? new ClaimsIdentity()));
    }

    public void NotifyUserAuthentication(string token)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(jwtToken.Claims, "jwtAuth"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        _serverHttpClient.DefaultRequestHeaders.Authorization = null;
        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        NotifyAuthenticationStateChanged(authState);
    }
}
