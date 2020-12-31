using EyE.Shared.Enums;
using EyE.Shared.Models.Common;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EyE.Shared.Helpers
{
    //https://issue.life/questions/8555320 - вопрос про API Википедии
    //https://en.wikipedia.org/w/api.php - сам API Википедии
    //https://en.wikipedia.org/api/rest_v1/#/Page%20content - Rest Api
    //https://json2csharp.com/  - Convert Json to C# Classes Online
    //Limit: none
    public static class WikiHelper
    {
        private const string pageSummaryRequestPattern = "/api/rest_v1/page/summary/";
        private const string randomPageSummaryRequestPattern = "/api/rest_v1/page/random/summary";
        private const string pageHtmlRequestPattern = "/api/rest_v1/page/html/";

        /// <param name="link">Например: https://ru.wikipedia.org/wiki/Ария_(группа) </param>
        public static async Task<WikiModel> GetPageSummaryAsync(string link, HttpClient client)
        {
            using var responseStream = await client.GetStreamAsync(GetBasePath(link) + pageSummaryRequestPattern + GetId(link));
            var summaryObject = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(responseStream);
            return GetSummary(summaryObject, link);
        }

        /// <param name="link">Например: https://ru.wikipedia.org </param>
        public static async Task<WikiModel> GetRandomPageSummaryAsync(string link, HttpClient client)
        {
            using var responseStream = await client.GetStreamAsync(link + randomPageSummaryRequestPattern);
            var summaryObject = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(responseStream);
            var canonicalTitle = summaryObject["titles"].EnumerateObject().First(obj => obj.Name == "canonical").Value;
            return GetSummary(summaryObject, $"{link}/wiki/{canonicalTitle}");
        }

        /// <param name="link">Например: https://ru.wikipedia.org/wiki/Ария_(группа) </param>
        public static async Task<HtmlDocument> GetPageHtmlAsync(string link, HttpClient client)
        {
            var responseStream = await client.GetStreamAsync(GetBasePath(link) + pageHtmlRequestPattern + GetId(link));
            var document = new HtmlDocument();
            document.Load(responseStream);
            return document;
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
            return link.Substring(0, link.IndexOf('/', 8));
        }
    }
}
