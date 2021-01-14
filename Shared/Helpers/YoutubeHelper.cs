using EyE.Shared.Models.Common;
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

        /// <param name="link">Например: https://www.youtube.com/watch?v=jNQXAC9IVRw </param>
        public static async Task<LinkModel> GetLinkModelAsync(string link, HttpClient client)
        {
            try
            {
                var responseStream = await client.GetStreamAsync(videoInfoRequestPattern + link);
                var youtubeObject = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(responseStream);

                return new LinkModel()
                {
                    Link = link,
                    Name = youtubeObject["title"].ToString(),
                    ImageSource = $"//img.youtube.com/vi/{LinkHelper.GetLinkParameter(link, "v")}/hqdefault.jpg"
                };
            }
            catch
            {
                await LoggingHelper.SendErrorAsync(link, client, typeof(YoutubeHelper).Name);
            }

            return default;
        }
    }
}
