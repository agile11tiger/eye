using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(EyE.Server.Areas.Identity.IdentityHostingStartup))]
namespace EyE.Server.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}