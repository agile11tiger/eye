using EyE.Shared.Converters;
using EyE.Shared.Extensions;
using EyE.Shared.Models.Common;
using EyE.Shared.Models.Review;
using EyE.Shared.ViewModels.Common.Interfaces;
using EyE.Shared.ViewModels.Review;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EyE.Shared.Helpers
{
    //https://rapidapi.com/blog/how-to-use-imdb-api/
    //http://www.omdbapi.com/
    //https://imdb-api.com/
    //Limit: 1000/day for omdb
    //Limit: 100/day for imdb
    public static class IMDbHelper
    {
        private const string omdbItemRequestPattern = "https://www.omdbapi.com/?apikey=29d4de6c&i=";
        private static string imdbItemRequestPattern = "https://imdb-api.com/en/API/Title/k_t2q0r4nq/";
        private static string imdbItemRequestReservePattern = "https://imdb-api.com/en/API/Title/k_38p3a9t7/";
        private static int keysCounter = 0;
        private const string imdbRequestParametersPattern = "/Images,Ratings,";
        private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters =
            {
                new BooleanConverter(),
                new DoubleConverter(),
                new DateTimeConverter(),
                new UInt16Converter(),
                new Int32Converter(),
            }
        };

        public const string BasePath = "https://imdb.com";
        public const string ImageRequestPattern = "https://m.media-amazon.com/images/M/";

        /// <param name="link">Например: https://www.imdb.com/title/tt0380510/?ref_=vp_back </param>
        public static async Task<IMDbModel> GetIMDbModelAsync<T>(string link, HttpClient client) where T : IMDbModel, new()
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

            return await GetDefaultIMDbModelAsync<T>(link, client);
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
                return "";
            }
        }

        private static async Task<T> GetDefaultIMDbModelAsync<T>(string link, HttpClient client) where T : IMDbModel, new()
        {
            var imdbId = GetId(link);
            return new T()
            {
                Link = link,
                Name = imdbId,
                IMDbId = imdbId,
                AddingDate = DateTime.Now,
                ImageSource = GetImageId(await GetImageSourceAsync(link, client))
            };
        }

        /// <param name="link">Например: https://www.imdb.com/title/tt0380510/?ref_=vp_back </param>
        private static async Task<FilmModel> GetFilmModelAsync(string link, HttpClient client)
        {
            var filmModel = new FilmModel();
            var imdbViewModel = await GetIMDbViewModelAsync(link, client);
            imdbViewModel.CopyProperties(filmModel);
            return filmModel;
        }

        /// <param name="link">Например: https://www.imdb.com/title/tt7569592/?ref_=nv_sr_srsg_0 </param>
        private static async Task<SerialModel> GetSerialModelAsync(string link, HttpClient client)
        {
            var serialModel = new SerialModel();
            var imdbViewModel = await GetIMDbViewModelAsync(link, client);
            imdbViewModel.CopyProperties(serialModel);
            return serialModel;
        }

        /// <param name="link">Например: https://www.imdb.com/title/tt2993508/?ref_=nv_sr_srsg_3 </param>
        private static async Task<GameModel> GetGameModelAsync(string link, HttpClient client)
        {
            var gameModel = new GameModel();
            var imdbViewModel = await GetIMDbViewModelAsync(link, client);
            imdbViewModel.CopyProperties(gameModel);
            return gameModel;
        }

        private static async Task<IIMDbViewModel> GetIMDbViewModelAsync(string link, HttpClient client)
        {
            IIMDbViewModel imdbViewModel = null;
            var imdbId = GetId(link);

            try
            {
                using var omdbResponseStream = await client.GetStreamAsync(omdbItemRequestPattern + imdbId);
                imdbViewModel = await JsonSerializer.DeserializeAsync<OMDbViewModel>(omdbResponseStream, serializerOptions);
            }
            catch (Exception e)
            {
                await LoggingHelper.SendErrorAsync(
                    $"Allowed error.\r\n{link}\r\nMessage:{e.Message}\r\n{e.StackTrace}", client, typeof(IMDbHelper).Name);
            }

            if (imdbViewModel == null || imdbViewModel.IsError || imdbViewModel.ImageSource == "N/A")
            {
                while (true)
                {
                    using var imdbResponseStream = await client.GetStreamAsync(imdbItemRequestPattern + imdbId + imdbRequestParametersPattern);
                    imdbViewModel = await JsonSerializer.DeserializeAsync<IMDbViewModel>(imdbResponseStream, serializerOptions);

                    if (imdbViewModel.Error.Contains("Maximum usage") && keysCounter < 1)
                    {
                        var temp = imdbItemRequestPattern;
                        imdbItemRequestPattern = imdbItemRequestReservePattern;
                        imdbItemRequestReservePattern = temp;
                        keysCounter++;
                        continue;
                    }

                    break;
                }

                if (imdbViewModel.IMDbId == default)
                    throw new HttpRequestException(imdbViewModel.Error);
            }

            imdbViewModel.ImageSource = GetImageId(imdbViewModel.ImageSource);
            imdbViewModel.Link = $"https://www.imdb.com/title/{imdbId}/";
            return imdbViewModel;
        }

        /// <param name="link">Например: https://imdb.com/title/tt0380510/?ref_=vp_back </param>
        /// <returns>Например: tt0380510</returns>
        private static string GetId(string link)
        {
            return LinkHelper.TrimProtocolName(link).Split('/')[2];
        }

        /// <param name="link">Например: https://imdb-api.com/images/original/MV5BODhjNjc1YjctZjZmZS00ZWM1LWFmZDYtYzFiZDg2ZDIyNDliXkEyXkFqcGdeQXVyNTc0NjY1ODk@._V1_Ratio0.7015_AL_.jpg </param>
        /// <returns>Например: MV5BODhjNjc1YjctZjZmZS00ZWM1LWFmZDYtYzFiZDg2ZDIyNDliXkEyXkFqcGdeQXVyNTc0NjY1ODk@._V1_Ratio0.7015_AL_.jpg</returns>
        private static string GetImageId(string link)
        {
            return link.Substring(link.LastIndexOf('/') + 1);
        }
    }
}
