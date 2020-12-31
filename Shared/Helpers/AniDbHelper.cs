﻿using EyE.Shared.Models.Review;
using System;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EyE.Shared.Helpers
{
    //https://wiki.anidb.net/HTTP_API_Definition#Clients
    //https://xmltocsharp.azurewebsites.net/
    //Если запросить один и тот же набор дважды, могут забанить.
    //Limit: 1page/2seccond
    public static class AniDbHelper
    {
        // http://api.anidb.net:9001/httpapi?protover=1&request=randomrecommendation&clientver=2&client=andrealcantara
        private const string pageInfoRequestPattern = "http://api.anidb.net:9001/httpapi?client=haruhibot&clientver=1&protover=1&request=anime&aid=";
        //https://cdn-eu.anidb.net/images/main/  - can`t loading src
        //https://cdn-eu.anidb.net/images/50x65  - good
        private const string imageRequestPattern = "https://cdn-eu.anidb.net/images/150/";
        public const string BasePath = "https://anidb.net";

        //Нельзя использовать на клиенте ошибка: Mixed Content
        public static async Task<AnimeModel> SetValuesAsync(AnimeModel model, HttpClient client, bool isGzip = true)
        {
            using var responseStream = await client.GetStreamAsync(pageInfoRequestPattern + model.AniDbId);
            using var decompressionStream = new GZipStream(responseStream, CompressionMode.Decompress);
            var animeXml = XDocument.Load(isGzip ? decompressionStream : responseStream).Element("anime");

            model.Type = animeXml.Element("type").Value;
            model.Episodecount = ushort.Parse(animeXml.Element("episodecount").Value);
            model.StartingDate = DateTime.Parse(animeXml.Element("startdate").Value);
            model.AddingDate = DateTime.Now;
            model.Name = animeXml
                .Element("titles")
                .Elements()
                .First(e => e.Attribute("{http://www.w3.org/XML/1998/namespace}lang").Value == "x-jat")
                .Value;
            model.Information = TakeAnimeDescription(animeXml.Element("description").Value).RemoveLinksAndSquareBrackets();
            model.AniDbRating = double.Parse(animeXml.Element("ratings").Element("permanent").Value, CultureInfo.InvariantCulture);
            model.ImageSource = imageRequestPattern + animeXml.Element("picture").Value;

            return model;
        }

        /// <param name="link">Например: https://anidb.net/anime/8691 или https://www.anidb.net/anime/8691 </param>
        /// <returns>Например: anidb.net/anime/8691 </returns>
        public static string GetShortLink(string link)
        {
            link = LinkHelper.TrimProtocolName(link);
            return LinkHelper.TrimStartLettersWWW(link);
        }

        /// <param name="link">Например: https://anidb.net/anime/8691 </param>
        /// <returns>Например: 8691</returns>
        public static string GetId(string link)
        {
            return LinkHelper.TrimProtocolName(link).Split('/')[2];
        }

        /// <summary>
        /// Берёт основную часть описания.
        /// </summary>
        private static string TakeAnimeDescription(string description)
        {
            foreach (var str in description.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var animeDescription = str.Trim();

                if (!animeDescription.StartsWith('*'))
                    return animeDescription;
            }

            return default;
        }

        /// <param name="str">Например: ... http://anidb.net/ch39512 [Haruyuki] ... </param>
        /// <returns>Например: ... Haruyuki ... </returns>
        private static string RemoveLinksAndSquareBrackets(this string str)
        {
            var pattern = @"(http.*\[)|\]|\[";
            return Regex.Replace(str, pattern, "");
        }
    }
}
