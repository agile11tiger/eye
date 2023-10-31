using Memory.ViewModels.Common.Interfaces;
using System.Text.Json.Serialization;
namespace Memory.ViewModels.Review;

public class OMDbViewModel : IIMDbViewModel
{
    public string? Link { get; set; }
    public string? Country { get; set; }
    public ushort Year { get; set; }
    public ushort Runtime { get; set; }
    public string? Genre { get; set; }
    public DateTime AddingDate { get => DateTime.Now; }
    public DateTime StartingDate { get => ReleasedDatetime == default ? ReleasedDatetime.AddYears(Year - 1) : ReleasedDatetime; }
    [JsonPropertyName("imdbID")] public string? IMDbId { get; set; }
    [JsonPropertyName("Title")] public string? Name { get; set; }
    [JsonPropertyName("Poster")] public string? ImageSource { get; set; }
    [JsonPropertyName("Released")] public DateTime ReleasedDatetime { get; set; }
    [JsonPropertyName("Plot")] public string? Information { get; set; }
    [JsonPropertyName("imdbRating")] public double IMDbRating { get; set; }
    [JsonPropertyName("imdbVotes")] public int IMDbVotes { get; set; }
    [JsonPropertyName("totalSeasons")] public ushort TotalSeasons { get; set; }

    /// <summary>
    /// Ошибка говорит о том, что ВСЕ данные не получены
    /// </summary>
    public string Error { get; set; } = string.Empty;
}
