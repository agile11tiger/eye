using EyE.Shared.Enums;
using EyE.Shared.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EyE.Server
{
    public static class RoleInitializer
    {
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync(Roles.Admin.ToString()) == null)
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

            if (await roleManager.FindByNameAsync(Roles.User.ToString()) == null)
                await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
        }

        public static async Task AddUserAsync(UserManager<User> userManager)
        {
            if (await userManager.FindByNameAsync(RoleInitializerData.EmailUser) == null)
            {
                var user = new User
                {
                    Email = RoleInitializerData.EmailUser,
                    UserName = RoleInitializerData.EmailUser,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(user, RoleInitializerData.PasswordUser);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, Roles.User.ToString());
            }
        }

        public static async Task AddAdminAsync(UserManager<User> userManager)
        {
            if (await userManager.FindByNameAsync(RoleInitializerData.EmailAdmin) == null)
            {
                var admin = new User
                {
                    Email = RoleInitializerData.EmailAdmin,
                    UserName = RoleInitializerData.EmailAdmin,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, RoleInitializerData.PasswordAdmin);

                if (result.Succeeded)
                    await userManager.AddToRolesAsync(admin, new[] { Roles.Admin.ToString() });
            }
        }
    }
}
