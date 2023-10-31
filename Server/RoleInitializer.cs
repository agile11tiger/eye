using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EyEServer;

public static class RoleInitializer
{
    public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.FindByNameAsync(Roles.Admin.ToString()) == null)
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

        if (await roleManager.FindByNameAsync(Roles.User.ToString()) == null)
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
    }

    public static async Task AddUserAsync(UserManager<UserModel> userManager)
    {
        if (await userManager.FindByNameAsync(RoleInitializerData.EMAIL_USER) == null)
        {
            var user = new UserModel
            {
                Email = RoleInitializerData.EMAIL_USER,
                UserName = RoleInitializerData.EMAIL_USER,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, RoleInitializerData.PASSWORD_USER);

            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, Roles.User.ToString());
        }
    }

    public static async Task AddAdminAsync(UserManager<UserModel> userManager)
    {
        if (await userManager.FindByNameAsync(RoleInitializerData.EMAIL_ADMIN) == null)
        {
            var admin = new UserModel
            {
                Email = RoleInitializerData.EMAIL_ADMIN,
                UserName = RoleInitializerData.EMAIL_ADMIN,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, RoleInitializerData.PASSWORD_ADMIN);

            if (result.Succeeded)
                await userManager.AddToRolesAsync(admin, new[] { Roles.Admin.ToString() });
        }
    }
}
