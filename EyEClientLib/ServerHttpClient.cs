
namespace EyEClientLib;

public class ServerHttpClient(HttpMessageHandler handler) : HttpClient(handler) 
{
    public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {

            return base.SendAsync(request, cancellationToken);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return null;
    }
}
