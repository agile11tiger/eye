using EyEServer.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
namespace EyEServer.Middlewares;

public class IpLoggerMiddleware
{
    private int _ipAddressesCounter;
    private int _uniqueIpAddressesCounter;
    private readonly RequestDelegate _next;
    private readonly string _ipAddressesFilePath;
    private readonly static object _locker = new();
    private readonly string _uniqueIpAddressesFilePath;
    private readonly HashSet<string> _uniqueIpAddresses = [];

    public IpLoggerMiddleware(RequestDelegate next)
    {
        _next = next;
        _ipAddressesFilePath = $"{LoggingController.LogDirectory}\\ipAddresses.log";
        _uniqueIpAddressesFilePath = $"{LoggingController.LogDirectory}\\uniqueIpAddresses.log";
    }

    public async Task InvokeAsync(HttpContext context)
    {
        lock (_locker)
        {
            var remoteIpAddress = context.Connection.RemoteIpAddress.ToString();

            if (!_uniqueIpAddresses.Contains(remoteIpAddress))
            {
                _uniqueIpAddresses.Add(remoteIpAddress);
                using var uniqueWriter = new StreamWriter(_uniqueIpAddressesFilePath, true, Encoding.UTF8);
                uniqueWriter.WriteLine($"{_uniqueIpAddressesCounter++}. {DateTime.Now}. {remoteIpAddress}");
            }

            using var writer = new StreamWriter(_ipAddressesFilePath, true, Encoding.UTF8);
            writer.WriteLine($"{_ipAddressesCounter++}. {DateTime.Now}. {remoteIpAddress}");
        }

        await _next.Invoke(context);
    }
}

public static class IpLoggerExtension
{
    public static IApplicationBuilder UseIpLogger(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IpLoggerMiddleware>();
    }
}
