using Microsoft.AspNetCore.Identity;

namespace EyE.Shared.Models.Identity
{
    public class UserModel : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
