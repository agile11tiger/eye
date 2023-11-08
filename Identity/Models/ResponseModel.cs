namespace Identity.Models;

public class ResponseModel : IDatabaseItem
{
    public int Id { get; set; }
    public string? Message { get; set; }
}
