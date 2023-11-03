using EyEServer.Services.RoleInitializer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
namespace EyEServer;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleInitializer = services.GetService<RoleInitializerService>();

            try
            {
                roleInitializer.InitializeAsync(services.GetRequiredService<RoleManager<IdentityRole>>()).Wait();
                roleInitializer.AddAdminAsync(services.GetRequiredService<UserManager<UserModel>>()).Wait();
                roleInitializer.AddUserAsync(services.GetRequiredService<UserManager<UserModel>>()).Wait();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
                return;
            }
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
