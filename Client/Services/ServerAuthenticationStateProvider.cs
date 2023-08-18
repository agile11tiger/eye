using EyE.Shared.Models.Common;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using EyE.Client.Extensions;
using System.IdentityModel.Tokens.Jwt;

namespace EyE.Client.Services
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        public ServerAuthenticationStateProvider(ServerHttpClient serverHttpClient, ILocalStorageService localStorage)
        {
            this.serverHttpClient = serverHttpClient;
            this.localStorage = localStorage;
        }

        private readonly ServerHttpClient serverHttpClient;
        private readonly ILocalStorageService localStorage;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userInfo = await localStorage.GetItemAsync<UserInfo>();
            ClaimsIdentity claimsIdentity = null;

            if (!string.IsNullOrWhiteSpace(userInfo?.Token))
            {
                serverHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", userInfo.Token);
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
            serverHttpClient.DefaultRequestHeaders.Authorization = null;
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
