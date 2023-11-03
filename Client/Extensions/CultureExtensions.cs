using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
namespace MemoryClient.Extensions;

public static class CultureExtensions
{
    public const string CURRENT_CULTURE = "currentCulture";

    public async static Task SetCultureFromStorage(this WebAssemblyHost host)
    {
        var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
        var currentCulture = await localStorage.GetItemAsync<string>(CURRENT_CULTURE);
        var cultureInfo = currentCulture != null ? new CultureInfo(currentCulture) : new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }

    public async static Task SetCultureAsync(this ILocalStorageService localStorage, CultureInfo cultureInfo)
    {
        Console.WriteLine("lol");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        await localStorage.SetItemAsync(CURRENT_CULTURE, cultureInfo.Name);
    }
}
