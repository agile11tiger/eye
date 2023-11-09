using MemoryLib.ViewModels.Common.Interfaces;
using System.Text.Json.Serialization;
namespace MemoryLib.ViewModels.Review;

public class IMDbViewModel : IIMDbViewModel
{
    public string? Link { get; set; }
    public DateTime AddingDate { get => DateTime.Now; }
    public DateTime StartingDate { get => ReleasedDatetime == default ? ReleasedDatetime.AddYears(Year - 1) : ReleasedDatetime; }
    public ushort TotalSeasons
    {
        get => (ushort)TvSeriesInfo!.Seasons!.FirstOrDefault();
        set => TvSeriesInfo = new TvSeriesInfo { Seasons = new List<int> { value } };
    }
    [JsonPropertyName("id")] public string? IMDbId { get; set; }
    [JsonPropertyName("title")] public string? Name { get; set; }
    [JsonPropertyName("countries")] public string? Country { get; set; }
    [JsonPropertyName("image")] public string? ImageSource { get; set; }
    [JsonPropertyName("year")] public ushort Year { get; set; }
    [JsonPropertyName("releaseDate")] public DateTime ReleasedDatetime { get; set; }
    [JsonPropertyName("runtimeMins")] public ushort Runtime { get; set; }
    [JsonPropertyName("genres")] public string? Genre { get; set; }
    [JsonPropertyName("plot")] public string? Information { get; set; }
    [JsonPropertyName("imDbRating")] public double IMDbRating { get; set; }
    [JsonPropertyName("imDbRatingVotes")] public int IMDbVotes { get; set; }
    [JsonPropertyName("tvSeriesInfo")] public TvSeriesInfo? TvSeriesInfo { get; set; }

    /// <summary>
    /// Ошибка говорит о том, что НЕКОТОРЫЕ данные не получены
    /// </summary>
    [JsonPropertyName("errorMessage")] public string Error { get; set; } = string.Empty;
}

public class TvSeriesInfo
{
    [JsonPropertyName("seasons")] public ICollection<int>? Seasons { get; set; }
}
