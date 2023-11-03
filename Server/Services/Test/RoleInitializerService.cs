using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
namespace EyEServer.Services.RoleInitializer;

public class RoleInitializerService(IOptions<RoleInitializerTestData> _roleInitializerData)
{
    public async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.FindByNameAsync(Roles.Admin.ToString()) == null)
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

        if (await roleManager.FindByNameAsync(Roles.User.ToString()) == null)
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
    }

    public async Task AddUserAsync(UserManager<UserModel> userManager)
    {
        if (await userManager.FindByNameAsync(_roleInitializerData.Value.UserEmail) == null)
        {
            var user = new UserModel
            {
                Email = _roleInitializerData.Value.UserEmail,
                UserName = _roleInitializerData.Value.UserEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, _roleInitializerData.Value.UserPassword);

            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, Roles.User.ToString());
        }
    }

    public async Task AddAdminAsync(UserManager<UserModel> userManager)
    {
        if (await userManager.FindByNameAsync(_roleInitializerData.Value.AdminEmail) == null)
        {
            var admin = new UserModel
            {
                Email = _roleInitializerData.Value.AdminEmail,
                UserName = _roleInitializerData.Value.AdminEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, _roleInitializerData.Value.AdminPassword);

            if (result.Succeeded)
                await userManager.AddToRolesAsync(admin, new[] { Roles.Admin.ToString() });
        }
    }
}
