namespace MemoryLib.Helpers;

public static class JsonHelper
{
    //Параметры по умолчанию не представлены в JsonSerializer.NET Core 3.1. 
    //Однако по состоянию на декабрь 2019 года это было добавлено в дорожную карту для 5.0.
    //https://stackoverflow.com/questions/58331479/how-to-globally-set-default-options-for-system-text-json-jsonserializer
    public static JsonSerializerOptions SerializeOptions = new()
    {
        IncludeFields = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

}
