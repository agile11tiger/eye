using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
namespace EyEServer.Services.Role;

public class RoleInitializerService(IOptions<RoleInitializerTestData> roleInitializerData)
{
    private readonly RoleInitializerTestData _roleInitializerData = roleInitializerData.Value;

    public async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.FindByNameAsync(Roles.Admin.ToString()) == null)
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

        if (await roleManager.FindByNameAsync(Roles.User.ToString()) == null)
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
    }

    public async Task AddUserAsync(UserManager<UserModel> userManager)
    {
        if (await userManager.FindByNameAsync(_roleInitializerData.UserEmail) == null)
        {
            var user = new UserModel
            {
                Email = _roleInitializerData.UserEmail,
                UserName = _roleInitializerData.UserEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, _roleInitializerData.UserPassword);

            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, Roles.User.ToString());
        }
    }

    public async Task AddAdminAsync(UserManager<UserModel> userManager)
    {
        if (await userManager.FindByNameAsync(_roleInitializerData.AdminEmail) == null)
        {
            var admin = new UserModel
            {
                Email = _roleInitializerData.AdminEmail,
                UserName = _roleInitializerData.AdminEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, _roleInitializerData.AdminPassword);

            if (result.Succeeded)
                await userManager.AddToRolesAsync(admin, new[] { Roles.Admin.ToString() });
        }
    }
}
