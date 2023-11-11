using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EyEServer.Services.Identity;

public class PasswordValidatorCustom : PasswordValidator<UserModel>
{
    public override Task<IdentityResult> ValidateAsync(UserManager<UserModel> manager, UserModel user, string password)
    {
        return Task.FromResult(IdentityResult.Success);
    }
}
