using EyE.Shared.Models.Common;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EyE.Shared.Helpers
{
    //https://github.com/zzzprojects/html-agility-pack - HAP
    public static class LinkHelper
    {
        private const string domainPattern = @"https?://(?:www\.|)([\w.-]+).*";

        /// <summary>
        /// Парсит сайт и забирает необходимые данные
        /// </summary>
        public static async Task SetNameAndFaviconAsync(LinkModel model)
        {
            var web = new HtmlWeb();;
            var htmlDoc = await web.LoadFromWebAsync(model.Link);
            var name = htmlDoc.DocumentNode.SelectSingleNode("//h1")
                ?.InnerText.Trim(' ', '\n', '\r')
                .Replace("&nbsp;", "\u00A0")
                ?? htmlDoc.DocumentNode.SelectSingleNode("//head/title")?.InnerText;
            model.Name = string.IsNullOrWhiteSpace(name) ? model.Link : name;
            model.ImageSource = "https://s2.googleusercontent.com/s2/favicons?domain=" + GetDomain(model.Link);
        }

        /// <summary>
        /// Парсит сайт и забирает необходимые данные
        /// </summary>
        public static async Task SetTitleAndImageAsync(LinkModel model)
        {
            var web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(model.Link);
            var title = htmlDoc.DocumentNode.SelectSingleNode("//head/title")?.InnerText.Replace("&nbsp;", "\u00A0");
            model.Name = string.IsNullOrWhiteSpace(title) ? model.Link : title;
            model.ImageSource = string.Empty;
        }

        /// <param name="link">Например: https://www.youtube.com/watch?v=5hVfxEc6WyY&list=WL&index=76 </param>
        /// <param name="key">Например: v</param>
        /// <returns>Например: 5hVfxEc6WyY</returns>
        public static string GetLinkParameter(string link, string key)
        {
            var array = link.Substring(link.IndexOf('?') + 1).Split('=', '&');

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

        /// <param name="link">Например: www.imdb.com</param>
        /// <returns>Например: imdb.com</returns>
        public static string TrimStartLettersWWW(string link)
        {
            return link.TrimStart('w', '.');
        }

        /// <param name="link">Например: https://www.imdb.com/title/tt1037705/?ref_=nv_sr_srsg_0 </param>
        /// <returns>Например: imdb.com</returns>
        public static string GetDomain(string link)
        {
            return Regex.Match(link, domainPattern).Groups[1].Value;
        }
    }
}
