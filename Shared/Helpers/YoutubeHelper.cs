using EyE.Shared.Models.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EyE.Shared.Helpers
{
    //https://stackoverflow.com/questions/30084140/youtube-video-title-with-api-v3-without-api-key/48884290     -get youtube video info 
    //http://shpargalkablog.ru/2013/06/youtube.html                                                             -get youtube video image
    //https://developers.google.com/youtube/player_parameters?hl=ru#showinfo                                    -youtube iframe parameters
    //https://habr.com/ru/post/488516/                                                                          -iframe parameters
    //https://snipp.ru/html-css/settings-youtube#link-knopka-polnoekrannogo-rezhima                             -iframe parameters
    //Limit: none
    public static class YoutubeHelper
    {
        private const string videoInfoRequestPattern = "https://noembed.com/embed?url=";
        public const string BasePath = "https://youtube.com";

        /// <param name="link">Например: https://youtu.be/4HohFXTbVtI?list=WL </param>
        public static async Task<LinkModel> GetLinkModelAsync(string link, HttpClient client)
        {
            var linkWithoutParameters = LinkHelper.RemoveRequestParameters(link);

            try
            {
                //некоторые ссылки без параметров блокируются
                var timeParameter = "?t=0";
                var responseStream = await client.GetStreamAsync(videoInfoRequestPattern + linkWithoutParameters + timeParameter);
                var youtubeObject = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(responseStream);

                return new LinkModel()
                {
                    Link = linkWithoutParameters,
                    Name = youtubeObject["title"].ToString(),
                    ImageSource = $"//img.youtube.com/vi/{GetId(link)}/hqdefault.jpg"
                };
            }
            catch (Exception e)
            {
                await LoggingHelper.SendErrorAsync(
                    $"{videoInfoRequestPattern + linkWithoutParameters}\r\nMessage:{e.Message}",
                    client,
                    typeof(YoutubeHelper).Name);
            }

            return default;
        }

        public static string GetId(string link)
        {
            link = LinkHelper.RemoveRequestParameters(link);
            return link.Substring(link.LastIndexOf('/') + 1);
        }
    }
}
