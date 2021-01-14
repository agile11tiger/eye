using EyE.Shared.Extensions;
using EyE.Shared.Models.Common;
using EyE.Shared.Models.Review;
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
        private const string imageRequestPattern = "https://img.omdbapi.com/?apikey=29d4de6c&i=";
        public const string BasePath = "https://imdb.com";

        /// <param name="link">Например: https://www.imdb.com/title/tt0380510/?ref_=vp_back </param>
        public static async Task<FilmModel> GetFilmModelAsync(string link, HttpClient client)
        {
            try
            {
                var filmModel = new FilmModel();
                var (imdbModel, _) = await GetModelAsync(link, client);
                imdbModel.CopyProperties(filmModel);
                return filmModel;
            }
            catch
            {
                await LoggingHelper.SendErrorAsync(link, client, typeof(IMDbHelper).Name);
            }

            return default;
        }

        /// <param name="link">Например: https://www.imdb.com/title/tt7569592/?ref_=nv_sr_srsg_0 </param>
        public static async Task<SerialModel> GetSerialModelAsync(string link, HttpClient client)
        {
            try
            {
                var serialModel = new SerialModel();
                var (imdbModel, imdbJObject) = await GetModelAsync(link, client);
                imdbModel.CopyProperties(serialModel);
                serialModel.TotalSeasons = ushort.Parse(imdbJObject["totalSeasons"].ToString());
                return serialModel;
            }
            catch
            {
                await LoggingHelper.SendErrorAsync(link, client, typeof(IMDbHelper).Name);
            }

            return default;
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

        private static async Task<(IMDbModel, Dictionary<string, JsonElement>)> GetModelAsync(string link, HttpClient client)
        {
            var responseStream = await client.GetStreamAsync(itemRequestPattern + GetId(link));
            var imdbObject = await JsonSerializer.DeserializeAsync<Dictionary<string, JsonElement>>(responseStream);
            DateTime.TryParse(imdbObject["Released"].ToString(), out var startingDate);
            ushort.TryParse(imdbObject["Runtime"].ToString().Split(' ').First(), out var runtime);
            double.TryParse(imdbObject["imdbRating"].ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var imdbRating);
            int.TryParse(imdbObject["imdbVotes"].ToString(), NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var imdbVotes);

            return (new IMDbModel()
            {
                Link = link,
                IMDbId = imdbObject["imdbID"].ToString(),
                Name = imdbObject["Title"].ToString(),
                ImageSource = imageRequestPattern + imdbObject["imdbID"].ToString(),
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
