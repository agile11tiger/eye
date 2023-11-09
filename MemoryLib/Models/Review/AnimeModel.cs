namespace MemoryLib.Models.Review;

public class AnimeModel : ReviewModel
{
    public string? AniDbId { get; set; }
    public string? Type { get; set; }
    public ushort Episodecount { get; set; }
    public double AniDbRating { get; set; }
    public int AniDbVotes { get; set; }
    public double MyRating { get; set; }
}
