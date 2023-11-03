using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
namespace MemoryClient.Handlers;

//https://www.meziantou.net/bypass-browser-cache-using-httpclient-in-blazor-webassembly.htm
public class DefaultBrowserOptionsMessageHandler : DelegatingHandler
{
    public DefaultBrowserOptionsMessageHandler()
    {
    }

    public DefaultBrowserOptionsMessageHandler(HttpMessageHandler innerHandler)
    {
        InnerHandler = innerHandler;
    }

    public BrowserRequestCache DefaultBrowserRequestCache { get; set; }
    public BrowserRequestCredentials DefaultBrowserRequestCredentials { get; set; }
    public BrowserRequestMode DefaultBrowserRequestMode { get; set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Get the existing options to not override them if set explicitly
        IDictionary<string, object> existingProperties = null;
        var requestOptionsKey = new HttpRequestOptionsKey<object>("WebAssemblyFetchOptions");

        if (request.Options.TryGetValue(requestOptionsKey, out object fetchOptions))
        {
            existingProperties = (IDictionary<string, object>)fetchOptions;
        }

        if (existingProperties?.ContainsKey("cache") != true)
        {
            request.SetBrowserRequestCache(DefaultBrowserRequestCache);
        }

        if (existingProperties?.ContainsKey("credentials") != true)
        {
            request.SetBrowserRequestCredentials(DefaultBrowserRequestCredentials);
        }

        if (existingProperties?.ContainsKey("mode") != true)
        {
            request.SetBrowserRequestMode(DefaultBrowserRequestMode);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
