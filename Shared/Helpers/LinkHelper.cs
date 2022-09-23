using EyE.Shared.Models.Common;
using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EyE.Shared.Helpers
{
    //https://github.com/zzzprojects/html-agility-pack - HAP
    public static class LinkHelper
    {
        private const string DOMAIN_PATTERN = @"https?://(?:www\.|)([\w.-]+).*";

        /// <summary>
        /// Парсит сайт и забирает необходимые данные
        /// </summary>
        public static async Task<bool> TrySetNameAndFaviconAsync(LinkModel model, HttpClient client)
        {
            try
            {
                var web = new HtmlWeb();
                var htmlDoc = await web.LoadFromWebAsync(model.Link);
                var name = htmlDoc.DocumentNode.SelectSingleNode("//head/title")?.InnerText 
                    ?? htmlDoc.DocumentNode.SelectSingleNode("//h1")?.InnerText;
                model.Name = string.IsNullOrWhiteSpace(name) 
                    ? model.Link 
                    : Regex.Replace(name, @"\s+", " ").Replace("&nbsp;", "\u00A0");
                model.ImageSource = "https://s2.googleusercontent.com/s2/favicons?domain=" + GetDomain(model.Link!);
                return true;
            }
            catch (Exception ex)
            {
                await LoggingHelper.SendErrorAsync($"{model.Link}\r\nMessage:{ex.Message}", client, typeof(LinkHelper).Name);
            }

            return false;
        }

        /// <summary>
        /// Парсит сайт и забирает необходимые данные
        /// </summary>
        public static async Task<bool> TrySetTitleAndImageAsync(LinkModel model, HttpClient client)
        {
            try
            {
                var web = new HtmlWeb();
                var htmlDoc = await web.LoadFromWebAsync(model.Link);
                var title = htmlDoc.DocumentNode.SelectSingleNode("//head/title")?.InnerText.Replace("&nbsp;", "\u00A0");
                model.Name = string.IsNullOrWhiteSpace(title) ? model.Link : title;
                model.ImageSource = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                await LoggingHelper.SendErrorAsync($"{model.Link}\r\nMessage:{ex.Message}", client, typeof(LinkHelper).Name);
            }

            return false;
        }

        public static string RemoveRequestParameters(string link)
        {
            var questionCharPosition = link.IndexOf('?');
            questionCharPosition = questionCharPosition == -1 ? link.Length : questionCharPosition;
            return link[..questionCharPosition];
        }

        /// <param name="link">Например: https://www.youtube.com/watch?v=5hVfxEc6WyY&list=WL&index=76 </param>
        /// <param name="key">Например: v</param>
        /// <returns>Например: 5hVfxEc6WyY</returns>
        public static string GetLinkParameter(string link, string key)
        {
            var array = link[(link.IndexOf('?') + 1)..].Split('=', '&');

            for (var i = 0; i < array.Length; i++)
            {
                if (array[i] == key)
                    return array[i + 1];
            }

            return string.Empty;
        }

        /// <param name="link">Например: https://www.imdb.com </param>
        /// <returns>Например: www.imdb.com</returns>
        public static string TrimProtocolName(string link)
        {
            return Regex.Replace(link, "https?://", string.Empty);
        }

        /// <param name="link">Например: https://www.imdb.com/title/tt1037705/?ref_=nv_sr_srsg_0 </param>
        /// <returns>Например: imdb.com</returns>
        public static string GetDomain(string link)
        {
            return Regex.Match(link, DOMAIN_PATTERN).Groups[1].Value;
        }
    }
}
