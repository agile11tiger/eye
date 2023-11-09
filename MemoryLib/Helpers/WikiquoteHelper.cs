namespace MemoryLib.Helpers;

public static class WikiquoteHelper
{
    private static readonly string[] SectionHeadersId = new[]
    {
        "Комментарии",
        "Источники",
        "Примечания",
        "Примечание",
        "См._также",
        "См.также",
        "Ссылки",
    };
    private static readonly string[] IdsToRemove = new[]
    {
        //например в конце https://ru.wikiquote.org/api/rest_v1/page/html/Льеж
        "social_bookmarks",
    };
    private const string QUOTES_FRAME_STYLE =
        "<style>" +
            "*{color:black !important; cursor:text !important; text-decoration:none !important; background:transparent !important;}" +
            "body {padding:5px !important; margin:0px !important; height:95%}" +
            "body>section:first-child>h2 {margin-top:0px; padding-top:0px;}" +
            "h3 {margin-top:0px; padding-top:0px;}" +
            "body::-webkit-scrollbar {width:3px;}" +
            "body::-webkit-scrollbar-track {background-color:transparent;}" +
            "body::-webkit-scrollbar-thumb {border-radius:3px; background-color: #aaa;}" +
        "</style>";

    public const string BASE_PATH = "https://ru.wikiquote.org";

    /// <param name="link">Например: https://ru.wikiquote.org/wiki/Конфуций или null </param>
    public static async Task<WikiquoteViewModel>? GetWikiquoteModelAsync(string link, HttpClient client)
    {
        try
        {
            var wikiModel = string.IsNullOrWhiteSpace(link)
                ? await WikiHelper.GetRandomPageSummaryAsync(BASE_PATH, client)!
                : await WikiHelper.GetPageSummaryAsync(link, client)!;

            var htmlDoc = await WikiHelper.GetPageHtmlAsync(BASE_PATH + "/wiki/" + wikiModel!.WikiId, client)!;

            return new WikiquoteViewModel()
            {
                Name = InsertWikipediaLink(wikiModel.Name!, htmlDoc),
                ImageSource = wikiModel.ImageSource,
                Information = wikiModel.Information,
                Quotes = GetQuotesText(htmlDoc),
                AddingDate = wikiModel.AddingDate,
            };
        }
        catch (Exception ex)
        {
            if (!string.IsNullOrWhiteSpace(link) && link.StartsWith("https://ru.wikiquote.org/wiki/"))
                await LoggingHelper.SendErrorAsync($"{link}\r\nMessage:{ex.Message}", client, typeof(WikiquoteHelper).Name);
        }

        return default!;
    }

    private static string InsertWikipediaLink(string name, HtmlDocument htmlDoc)
    {
        var wikipediaLink = (htmlDoc.DocumentNode.SelectSingleNode("//span[@id='wikipedia-ref']/a")
            ?? htmlDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'wikipedia-ref')]/a")
            ?? htmlDoc.DocumentNode.SelectSingleNode("//a[.='Статья в Википедии']")  //оба пробела: "20"
            ?? htmlDoc.DocumentNode.SelectSingleNode("//a[.='Статья в Википедии']")  //первый пробел: "20". второй пробел: "c2a0"
            )?.Attributes["href"].Value.ToString();

        if (!string.IsNullOrEmpty(wikipediaLink))
            return $"<a href='{wikipediaLink}' target='_blank'>{name}</a>";

        return name;
    }

    /// <param name="link">Например: https://ru.wikiquote.org/wiki/Конфуций </param>
    private static string GetQuotesText(HtmlDocument htmlDoc)
    {
        if (htmlDoc.DocumentNode.SelectSingleNode("//body").ChildNodes.Count > 1)
            htmlDoc.DocumentNode.SelectSingleNode("//body").FirstChild.Remove();
        else
        {
            var childNodes = htmlDoc.DocumentNode.SelectSingleNode("//body").FirstChild.ChildNodes;
            for (var i = 0; i < childNodes.Count; i++)
            {
                if (childNodes[i].OriginalName == "table" || childNodes[i].OriginalName == "ul")
                    continue;

                childNodes[i]?.Remove();
                i--;
            }
        }

        var empty = Enumerable.Empty<HtmlNode>();

        foreach (var header in SectionHeadersId)
            htmlDoc.GetElementbyId(header)?.ParentNode.Remove();

        foreach (var sup in htmlDoc.DocumentNode.SelectNodes("//figure") ?? empty)
            sup.Remove();

        foreach (var sup in htmlDoc.DocumentNode.SelectNodes("//sup") ?? empty)
            sup.Remove();

        foreach (var a in htmlDoc.DocumentNode.SelectNodes("//a") ?? empty)
            a.Attributes["href"]?.Remove();

        foreach (var header in IdsToRemove)
            htmlDoc.GetElementbyId(header)?.Remove();

        //например: https://ru.wikiquote.org/api/rest_v1/page/html/Санкт-Петербург
        foreach (var navbox in htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'navbox')]") ?? empty)
            navbox.Remove();

        //панелька навигации по алфавиту
        //например: https://ru.wikiquote.org/api/rest_v1/page/html/Игнатий%20(Брянчанинов)
        foreach (var navbox in htmlDoc.DocumentNode.SelectNodes("//table[contains(@class, 'toccolours')]") ?? empty)
            navbox.Remove();

        //например: https://ru.wikiquote.org/api/rest_v1/page/html/Генри%20Джеймс
        foreach (var navbox in htmlDoc.DocumentNode.SelectNodes("//*[contains(@class, 'metadata')]") ?? empty)
            navbox.Remove();

        //например: https://ru.wikiquote.org/api/rest_v1/page/html/Франсуа%20де%20Ларошфуко
        foreach (var table in htmlDoc.DocumentNode.SelectNodes("//table[contains(@class, 'cleanuprewrite')]") ?? empty)
            table.Remove();

        //например: https://ru.wikiquote.org/api/rest_v1/page/html/Франьо%20Туджман
        foreach (var div in htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'infobox')]") ?? empty)
            div.Remove();

        //пустые пункты(черные точки без текста)
        foreach (var navbox in htmlDoc.DocumentNode.SelectNodes("//li[contains(@class, 'mw-empty-elt')]") ?? empty)
            navbox.Remove();

        var style = HtmlNode.CreateNode(QUOTES_FRAME_STYLE);
        htmlDoc.DocumentNode.SelectSingleNode("//head").AppendChild(style);

        return htmlDoc.DocumentNode.OuterHtml;
    }
}
