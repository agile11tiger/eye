using EyE.Shared.Extensions;
using EyE.Shared.Models.Common;
using EyE.Shared.Models.Review;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EyE.Shared.Helpers
{
    //https://rapidapi.com/blog/how-to-use-imdb-api/
    //http://www.omdbapi.com/
    //Limit: 1000/day
    public static class IMDbHelper
    {
        private const string itemRequestPattern = "https://www.omdbapi.com/?apikey=29d4de6c&i=";
        //private const string imageRequestPattern = "https://img.omdbapi.com/?apikey=29d4de6c&i=";
        public const string BasePath = "https://imdb.com";

        /// <param name="link">Например: https://www.imdb.com/title/tt0380510/?ref_=vp_back </param>
        public static async Task<IMDbModel> GetIMDbModelAsync<T>(string link, HttpClient client) where T: IMDbModel, new()
        {
            try
            {
                if (typeof(T).IsAssignableFrom(typeof(FilmModel)))
                    return await GetFilmModelAsync(link, client);
                else if (typeof(T).IsAssignableFrom(typeof(SerialModel)))
                    return await GetSerialModelAsync(link, client);
                else if (typeof(T).IsAssignableFrom(typeof(GameModel)))
                    return await GetGameModelAsync(link, client);
            }
            catch (Exception e)
            {
                await LoggingHelper.SendErrorAsync($"{link}\r\nMessage:{e.Message}\r\n{e.StackTrace}", client, typeof(IMDbHelper).Name);
            }

            var imdbId = GetId(link);
            return new T()
            {
                Link = link,
                Name = imdbId,
                IMDbId = imdbId,
                AddingDate = DateTime.Now,
                ImageSource = await GetImageSourceAsync(link, client)
            };
        }

        /// <param name="link">Например: https://imdb.com/title/tt0380510/?ref_=vp_back 
        /// или https://www.imdb.com/title/tt0380510/?ref_=vp_back </param>
        /// <returns>Например: imdb.com/title/tt0380510 </returns>
        public static string GetShortLink(string link)
        {
            link = LinkHelper.TrimProtocolName(link);
            link = LinkHelper.TrimStartLettersWWW(link);
            return link.Substring(0, link.LastIndexOf('/'));
        }

        /// <summary>
        /// Парсит сайт и забирает ссылку на изображение
        /// </summary>
        /// <returns>string if successful, otherwise empty string</returns>
        public static async Task<string> GetImageSourceAsync(string link, HttpClient client)
        {
            try
            {
                var web = new HtmlWeb();
                var htmlDoc = await web.LoadFromWebAsync(link);
                return htmlDoc
                    .DocumentNode
                    .SelectSingleNode("//div[@class='poster']//img")
                    .GetAttributeValue("src", string.Empty);
            }
            catch (Exception e)
            {
                await LoggingHelper.SendErrorAsync($"{link}\r\nMessage:{e.Message}", client, typeof(IMDbHelper).Name);
                return "N/A";
            }
        }

        /// <param name="link">Например: https://www.imdb.com/title/tt0380510/?ref_=vp_back </param>
        private static async Task<FilmModel> GetFilmModelAsync(string link, HttpClient client)
        {
            var filmModel = new FilmModel();
            var (imdbModel, _) = await GetModelAsync(link, client);
            imdbModel.CopyProperties(filmModel);
            return filmModel;
        }

        /// <param name="link">Например: https://www.imdb.com/title/tt7569592/?ref_=nv_sr_srsg_0 </param>
        private static async Task<SerialModel> GetSerialModelAsync(string link, HttpClient client)
        {
            var serialModel = new SerialModel();
            var (imdbModel, imdbJObject) = await GetModelAsync(link, client);
            imdbModel.CopyProperties(serialModel);
            serialModel.TotalSeasons = ushort.Parse(imdbJObject["totalSeasons"].ToString());
            return serialModel;
        }

        /// <param name="link">Например: https://www.imdb.com/title/tt2993508/?ref_=nv_sr_srsg_3 </param>
        private static async Task<GameModel> GetGameModelAsync(string link, HttpClient client)
        {
            var gameModel = new GameModel();
            var (imdbModel, _) = await GetModelAsync(link, client);
            imdbModel.CopyProperties(gameModel);
            return gameModel;
        }

        private static async Task<(IMDbModel, Dictionary<string, JsonElement>)> GetModelAsync(string link, HttpClient client)
        {
            var responseStream = await client.GetStreamAsync(itemRequestPattern + GetId(link));
            var imdbObject = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(responseStream);

            if (imdbObject["Response"].ToString() == "False")
                throw new HttpRequestException(imdbObject["Error"].ToString());

            DateTime.TryParse(imdbObject["Released"].ToString(), out var startingDate);
            ushort.TryParse(imdbObject["Runtime"].ToString().Split(' ').First(), out var runtime);
            double.TryParse(imdbObject["imdbRating"].ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var imdbRating);
            int.TryParse(imdbObject["imdbVotes"].ToString(), NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var imdbVotes);
            var poster = imdbObject["Poster"].ToString();

            if (poster == "N/A")
                poster = await GetImageSourceAsync(link, client);

            return (new IMDbModel()
            {
                Link = link,
                IMDbId = imdbObject["imdbID"].ToString(),
                Name = imdbObject["Title"].ToString(),
                Country = imdbObject["Country"].ToString(),
                ImageSource = poster,
                StartingDate = startingDate,
                AddingDate = DateTime.Now,

                Runtime = runtime,
                Genre = imdbObject["Genre"].ToString(),
                Information = imdbObject["Plot"].ToString(),
                IMDbRating = imdbRating,
                IMDbVotes = imdbVotes,
            }, imdbObject);
        }

        /// <param name="link">Например: https://imdb.com/title/tt0380510/?ref_=vp_back </param>
        /// <returns>Например: tt0380510</returns>
        private static string GetId(string link)
        {
            return LinkHelper.TrimProtocolName(link).Split('/')[2];
        }
    }
}
