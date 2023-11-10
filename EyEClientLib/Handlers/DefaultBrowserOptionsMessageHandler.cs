using EyEClientLib.Resources.Localization.Identity;
using EyEClientLib.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using MudBlazor;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
namespace EyEClientLib.Handlers;

//https://www.meziantou.net/bypass-browser-cache-using-httpclient-in-blazor-webassembly.htm
public class DefaultBrowserOptionsMessageHandler : DelegatingHandler
{
    public DefaultBrowserOptionsMessageHandler(
        ISnackbar snackbar,
        LoggerService loggerService,
        HttpClientHandler innerHandler)
    {
        _snackbar = snackbar;
        _loggerService = loggerService;
        InnerHandler = innerHandler;
    }

    private readonly ISnackbar _snackbar;
    private readonly LoggerService _loggerService;

    public BrowserRequestMode DefaultBrowserRequestMode { get; set; } = BrowserRequestMode.Cors;
    public BrowserRequestCache DefaultBrowserRequestCache { get; set; }
    public BrowserRequestCredentials DefaultBrowserRequestCredentials { get; set; }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AddHeaders(request);

        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            if (ex.HttpRequestError == HttpRequestError.Unknown)
            {
                _loggerService.Log(ex);
                cancellationToken.ThrowIfCancellationRequested();
                _snackbar.Add(IdentityResource.CheckInternetConnection, Severity.Error);
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.ServiceUnavailable };
            }
            else
                throw;
        }
    }

    private void AddHeaders(HttpRequestMessage request)
    {
        request.Headers.AcceptLanguage.Clear();
        request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.Name));
        // Get the existing options to not override them if set explicitly
        var requestOptionsKey = new HttpRequestOptionsKey<object>("WebAssemblyFetchOptions");

        if (request.Options.TryGetValue(requestOptionsKey, out object fetchOptions))
        {
            var existingProperties = (IDictionary<string, object>)fetchOptions;

            if (existingProperties.ContainsKey("cache") != true)
                request.SetBrowserRequestCache(DefaultBrowserRequestCache);

            if (existingProperties.ContainsKey("credentials") != true)
                request.SetBrowserRequestCredentials(DefaultBrowserRequestCredentials);

            if (existingProperties.ContainsKey("mode") != true)
                request.SetBrowserRequestMode(DefaultBrowserRequestMode);
        }
    }
}
