namespace MemoryClient.Handlers;

/// <summary>
/// A <see cref="DelegatingHandler"/> that attaches access tokens to outgoing <see cref="HttpResponseMessage"/> instances.
/// Access tokens will only be added when the request URI is within the application's base URI.
/// </summary>
// BaseAddressAuthorizationMessageHandler https://github.com/dotnet/aspnetcore/blob/b795ac3546eb3e2f47a01a64feb3020794ca33bb/src/Components/WebAssembly/WebAssembly.Authentication/src/Services/BaseAddressAuthorizationMessageHandler.cs
// AuthorizationMessageHandler https://github.com/dotnet/aspnetcore/blob/b795ac3546eb3e2f47a01a64feb3020794ca33bb/src/Components/WebAssembly/WebAssembly.Authentication/src/Services/AuthorizationMessageHandler.cs
//public class BaseAuthorizationMessageHandler : AuthorizationMessageHandler
//{
//    /// <summary>
//    /// Initializes a new instance of <see cref="BaseAddressAuthorizationMessageHandler"/>.
//    /// </summary>
//    /// <param name="provider">The <see cref="IAccessTokenProvider"/> to use for requesting tokens.</param>
//    /// <param name="navigationManager">The <see cref="NavigationManager"/> used to compute the base address.</param>
//    public BaseAuthorizationMessageHandler(IConfiguration configuration, IAccessTokenProvider provider, NavigationManager navigationManager)
//        : base(provider, navigationManager)
//    {
//        ConfigureHandler(new[] { configuration["ServerUri"] });
//    }
//}
