using EyE.Shared.Models.Common;
using EyE.Shared.ViewModels.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace EyE.Shared.ViewModels.Review
{
    public class IMDbViewModel: IIMDbViewModel
    {
        public string Link { get; set; }
        public DateTime AddingDate { get => DateTime.Now; }
        public DateTime StartingDate { get => ReleasedDatetime == default ? ReleasedDatetime.AddYears(Year) : ReleasedDatetime; }
        public ushort TotalSeasons { get => (ushort)TvSeriesInfo.Seasons.FirstOrDefault(); set => Enumerable.Range(1, value - 1); }
        [JsonPropertyName("id")] public string IMDbId { get; set; }
        [JsonPropertyName("title")] public string Name { get; set; }
        [JsonPropertyName("countries")] public string Country { get; set; }
        [JsonPropertyName("image")] public string ImageSource { get; set; }
        [JsonPropertyName("year")] public ushort Year { get; set; }
        [JsonPropertyName("releaseDate")] public DateTime ReleasedDatetime { get; set; }
        [JsonPropertyName("runtimeMins")] public ushort Runtime { get; set; }
        [JsonPropertyName("genres")] public string Genre { get; set; }
        [JsonPropertyName("plot")] public string Information { get; set; }
        [JsonPropertyName("imDbRating")] public double IMDbRating { get; set; }
        [JsonPropertyName("imDbRatingVotes")] public int IMDbVotes { get; set; }
        [JsonPropertyName("tvSeriesInfo")] public TvSeriesInfo TvSeriesInfo { get; set; }

        [JsonPropertyName("errorMessage")] public string Error { get; set; }
    }

    public class TvSeriesInfo
    {
        [JsonPropertyName("seasons")] public ICollection<int> Seasons { get; set; }
    }
}
