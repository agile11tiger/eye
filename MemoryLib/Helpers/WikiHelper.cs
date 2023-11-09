namespace MemoryLib.Helpers;

//https://issue.life/questions/8555320 - вопрос про API Википедии
//https://en.wikipedia.org/w/api.php - сам API Википедии
//https://en.wikipedia.org/api/rest_v1/#/Page%20content - Rest Api
//https://json2csharp.com/  - Convert Json to C# Classes Online
//Limit: none
public static class WikiHelper
{
    private const string PAGE_HTML_REQUEST_PATTERN = "/api/rest_v1/page/html/";
    private const string PAGE_SUMMARY_REQUEST_PATTERN = "/api/rest_v1/page/summary/";
    private const string RANDOM_PAGE_SUMMARY_REQUEST_PATTERN = "/api/rest_v1/page/random/summary";

    /// <param name="link">Например: https://ru.wikipedia.org/wiki/Ария_(группа) </param>
    public static async Task<WikiModel>? GetPageSummaryAsync(string link, HttpClient client)
    {
        try
        {
            using var responseStream = await client.GetStreamAsync(GetBasePath(link) + PAGE_SUMMARY_REQUEST_PATTERN + GetId(link));
            var summaryObject = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(responseStream);
            return GetSummary(summaryObject!, link);
        }
        catch (Exception ex)
        {
            await LoggingHelper.SendErrorAsync($"{link}\r\nMessage:{ex.Message}", client, typeof(WikiHelper).Name);
        }

        return default!;
    }

    /// <param name="link">Например: https://ru.wikipedia.org </param>
    public static async Task<WikiModel>? GetRandomPageSummaryAsync(string link, HttpClient client)
    {
        try
        {
            using var responseStream = await client.GetStreamAsync(link + RANDOM_PAGE_SUMMARY_REQUEST_PATTERN);
            var summaryObject = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(responseStream);
            var canonicalTitle = summaryObject!["titles"].EnumerateObject().First(obj => obj.Name == "canonical").Value;
            return GetSummary(summaryObject, $"{link}/wiki/{canonicalTitle}");
        }
        catch (Exception ex)
        {
            await LoggingHelper.SendErrorAsync($"{link}\r\nMessage:{ex.Message}", client, typeof(WikiHelper).Name);
        }

        return default!;
    }

    /// <param name="link">Например: https://ru.wikipedia.org/wiki/Ария_(группа) </param>
    public static async Task<HtmlDocument>? GetPageHtmlAsync(string link, HttpClient client)
    {
        try
        {
            var responseStream = await client.GetStreamAsync(GetBasePath(link) + PAGE_HTML_REQUEST_PATTERN + GetId(link));
            var document = new HtmlDocument();
            document.Load(responseStream);
            return document;
        }
        catch (Exception ex)
        {
            await LoggingHelper.SendErrorAsync($"{link}\r\nMessage:{ex.Message}", client, typeof(WikiHelper).Name);
        }

        return default!;
    }

    private static WikiModel GetSummary(Dictionary<string, JsonElement> summaryObject, string link)
    {
        return new WikiModel()
        {
            Link = link,
            WikiId = summaryObject["titles"].EnumerateObject().First(obj => obj.Name == "canonical").Value.ToString(),
            Name = summaryObject["displaytitle"].ToString(),
            ImageSource = summaryObject["thumbnail"].EnumerateObject().FirstOrDefault(obj => obj.Name == "source").Value.ToString(),
            OriginalImageSource = summaryObject["originalimage"].EnumerateObject().FirstOrDefault(obj => obj.Name == "source").Value.ToString(),
            Information = summaryObject["extract"].ToString(),
            AddingDate = DateTime.Now,
        };
    }

    /// <param name="link">Например: https://ru.wikipedia.org/wiki/Ария_(группа) </param>
    /// <returns>Например: Ария_(группа)</returns>
    private static string GetId(string link)
    {
        return LinkHelper.TrimProtocolName(link).Split('/')[2];
    }

    /// <param name="link">Например: https://ru.wikipedia.org/wiki/Ария_(группа) </param>
    /// <returns>Например: https://ru.wikipedia.org</returns>
    private static string GetBasePath(string link)
    {
        return link[..link.IndexOf('/', 8)];
    }
}
