namespace EyEClientLib;

public class ServerHttpClient(HttpMessageHandler handler) : HttpClient(handler) { }
