using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EyEServer.Services.Identity;

public class UsernameValidatorCustom : IUserValidator<UserModel>
{
    public Task<IdentityResult> ValidateAsync(UserManager<UserModel> manager, UserModel user)
    {
        return Task.FromResult(IdentityResult.Success);
    }
}
