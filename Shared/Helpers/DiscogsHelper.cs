using EyE.Shared.Models.Common;
using EyE.Shared.Models.Review;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EyE.Shared.Helpers
{
    //https://en.wikipedia.org/wiki/List_of_online_music_databases
    //https://www.discogs.com/developers#page:home,header:home-general-information  - api
    //Limit: 25/minute
    public static class DiscogsHelper
    {
        private const string artistsRequestPattern = "https://api.discogs.com/artists/";
        public const string BasePath = "https://discogs.com";

        /// <param name="link">Например: https://www.discogs.com/artist/484423-Ария </param>
        public static async Task<MusicModel> GetMusicModelAsync(string link, HttpClient client)
        {
            try
            {
                var id = GetId(link);
                using var responseStream = await client.GetStreamAsync(artistsRequestPattern + id);
                var artistObject = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(responseStream);
                using var stream = await client.GetStreamAsync($"https://api.discogs.com/artists/{id}/releases?page=1&per_page=1");
                var artistFirstReleaseObject = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(stream);
                var dateFirstRealease = artistFirstReleaseObject["releases"][0].EnumerateObject().First(obj => obj.Name == "year").Value;

                return new MusicModel()
                {
                    Link = link,
                    DiscogsId = artistObject["id"].ToString(),
                    Name = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(artistObject["name"].ToString())),
                    StartingDate = DateTime.Parse($"1/1/{dateFirstRealease}"),
                    AddingDate = DateTime.Now,
                    Sites = artistObject.TryGetValue("urls", out var jsonElement) ? jsonElement.ToString() : "[]",
                    Information = artistObject["profile"].ToString().RemoveLinksAndSquareBrackets()
                };
            }
            catch (Exception e)
            {
                await LoggingHelper.SendErrorAsync($"{link}\r\nMessage:{e.Message}", client, typeof(DiscogsHelper).Name);
            }

            return default;
        }

        /// <summary>
        /// Парсит сайт и забирает ссылку на изображение
        /// </summary>
        public static async Task SetImageSourceAsync(LinkModel model, HttpClient client)
        {
            try
            {
                var web = new HtmlWeb();
                var htmlDoc = await web.LoadFromWebAsync(model.Link);
                var image = htmlDoc
                    .DocumentNode
                    .SelectSingleNode("//span[@class='thumbnail_center']/img")
                    .GetAttributeValue("src", string.Empty);
                model.ImageSource = image;
            }
            catch (Exception e)
            {
                await LoggingHelper.SendErrorAsync($"{model.Link}\r\nMessage:{e.Message}", client, typeof(DiscogsHelper).Name);
            }
        }

        /// <param name="link">Например: https://discogs.com/artist/484423-Ария </param>
        /// <returns>Например: 484423</returns>
        private static string GetId(string link)
        {
            return LinkHelper.TrimProtocolName(link).Split('/')[2].Split('-')[0];
        }

        /// <param name="str">Например: ... [a = Ария] ... </param>
        /// <returns>Например: ... Ария ... </returns>
        private static string RemoveLinksAndSquareBrackets(this string str)
        {
            var pattern = @"(a=)|\]|\[";
            return Regex.Replace(str, pattern, "");
        }
    }
}
