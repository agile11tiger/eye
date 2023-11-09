namespace MemoryLib.Models.Common;

public class ReviewModel : LinkModel
{
    public string? Information { get; set; }
    public DateTime AddingDate { get; set; }
    public DateTime StartingDate { get; set; }
    public string? Comment { get; set; }
}
