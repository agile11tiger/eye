using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyE.Server.Middlewares
{
    public class IpLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string ipAddressesFilePath;
        private static readonly object locker = new object();
        private readonly HashSet<string> uniqueIpAddresses = new HashSet<string>();
        private int counter;

        public IpLoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
            ipAddressesFilePath = Environment.CurrentDirectory + @"\Logs\ipAddresses.log";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            lock (locker)
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress.ToString();

                if (!uniqueIpAddresses.Contains(remoteIpAddress))
                {
                    uniqueIpAddresses.Add(remoteIpAddress);
                    using var writer = new StreamWriter(ipAddressesFilePath, true, Encoding.UTF8);
                    writer.WriteLine($"{counter++}. {DateTime.Now}. {remoteIpAddress}");
                }
            }

            await next.Invoke(context);
        }
    }

    public static class IpLoggingExtension
    {
        public static IApplicationBuilder UseIpLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<IpLoggingMiddleware>();
        }
    }
}
