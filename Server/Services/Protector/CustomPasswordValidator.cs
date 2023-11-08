using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EyEServer.Services.Protector
{
    public class CustomPasswordValidator : PasswordValidator<UserModel>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<UserModel> manager, UserModel user, string password)
        {
            return IdentityResult.Success;
        }
    }
}
