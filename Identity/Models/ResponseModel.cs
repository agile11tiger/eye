namespace Identity.Models;

public class ResponseModel : IDatabaseItem
{
    public int Id { get; set; }
    public IEnumerable<string>? Messages { get; set; }
}
