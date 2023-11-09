using Microsoft.AspNetCore.Identity;
namespace IdentityLib.Models;

public class UserModel : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
