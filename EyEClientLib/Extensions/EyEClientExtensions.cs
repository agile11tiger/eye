using EyEClientLib.Components;
using EyEClientLib.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyEClientLib.Extensions
{
    public static class EyEClientExtensions
    {
        public static IServiceCollection AddEyEClientServices(this IServiceCollection services)
        {
            services.AddSingleton<LoggerService>();
            return services;
        }
    }
}
