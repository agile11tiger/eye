namespace Identity.Models;

public class LoginResponseModel : IdentityResponseModel
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
}
