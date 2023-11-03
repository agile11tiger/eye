using Microsoft.AspNetCore.Identity;
namespace Identity.Models;

public class UserModel : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
