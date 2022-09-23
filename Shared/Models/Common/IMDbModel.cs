namespace EyE.Shared.Models.Common
{
    public class IMDbModel : ReviewModel
    {
        public string? IMDbId { get; set; }
        public string? Country { get; set; }
        public ushort Runtime { get; set; }
        public string? Genre { get; set; }
        public double IMDbRating { get; set; }
        public int IMDbVotes { get; set; }
    }
}
