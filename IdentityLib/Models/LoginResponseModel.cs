namespace IdentityLib.Models;

public class LoginResponseModel : IdentityResponseModel
{
    public string Nickname { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
