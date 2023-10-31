namespace Identity.ViewModels;

public class RefreshTokenViewModel
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public IEnumerable<string>? Messages { get; set; }
}
