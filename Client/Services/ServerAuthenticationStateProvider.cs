namespace EyE.Client.Services
{
    //https://www.codewithmukesh.com/blog/authentication-in-blazor-webassembly/                           ---Индус
    //https://guidnew.com/en/blog/secure-a-blazor-webassembly-application-with-cookie-authentication/     ---Сеньор
    //https://jasonwatmore.com/post/2020/08/13/blazor-webassembly-jwt-authentication-example-tutorial     ---JWT Authentication
    //public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    //{
    //    //как реализован в блазоре
    //    //https://github.com/dotnet/aspnetcore/blob/057ec9cfae1831f91f1a0e01397059b20431db7b/src/Components/Server/src/Circuits/ServerAuthenticationStateProvider.cs
    //    public ServerAuthenticationStateProvider(PublicHttpClient client)
    //    {
    //        this.client = client;
    //    }

    //    private readonly PublicHttpClient client;
    //    private UserInfo userInfo;

    //    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    //    {
    //        ClaimsIdentity identity = null;
    //        await RefreshUserInfo();

    //        if (userInfo.IsAuthenticated)
    //        {
    //            var claims = new[]
    //                { new Claim(ClaimTypes.Name, userInfo.UserName) }
    //                .Concat(userInfo.Claims.Select(c => new Claim(c.Key, c.Value)));

    //            identity = new ClaimsIdentity(claims, "Server authentication");
    //        }

    //        return new AuthenticationState(new ClaimsPrincipal(identity ?? new ClaimsIdentity()));
    //    }

    //    public void StateChanged(bool needDeleteCacheUserInfo = false)
    //    {
    //        if (needDeleteCacheUserInfo)
    //            userInfo = null;

    //        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    //    }

    //    private async Task RefreshUserInfo(bool useCache = true)
    //    {
    //        if (useCache && userInfo != null && userInfo.IsAuthenticated)
    //            return;

    //        userInfo = await Client.GetFromJsonAsync<UserInfo>("account/getUserInfo");
    //    }
    //}
}
