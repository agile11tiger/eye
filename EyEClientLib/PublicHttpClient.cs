namespace EyEClientLib;
public class PublicHttpClient(HttpMessageHandler handler) : HttpClient(handler) 
{
}
