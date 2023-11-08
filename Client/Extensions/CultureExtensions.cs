using Blazored.LocalStorage;
using MemoryClient.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq;
namespace MemoryClient.Extensions;

public static class CultureExtensions
{
    public const string IS_FIRST_RUN = "isFirstRun";
    public const string CURRENT_CULTURE = "currentCulture";

    public async static Task SetCultureFromStorageAsync(this WebAssemblyHost host)
    {
        var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
        var isFirstRun = await localStorage.GetItemAsync<string>(IS_FIRST_RUN);

        if (isFirstRun == null)
        {
            //default browser set header example "Accept-Language: en-US" and this, i think, current CultureInfo.CurrentCulture.Name 
            var currentLanguageData = LanguageSelector.SupportedLanguages.FirstOrDefault(lang => lang.CultureInfo.Name == CultureInfo.CurrentCulture.Name);
            var currentCultureInfo = currentLanguageData != null ? currentLanguageData.CultureInfo : new CultureInfo("en-US");
            await localStorage.SetCultureAsync(currentCultureInfo);
            await localStorage.SetItemAsync(IS_FIRST_RUN, false);
        }
        else
        {
            var cultureInfo = new CultureInfo(await localStorage.GetItemAsync<string>(CURRENT_CULTURE));
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }

    public async static Task SetCultureAsync(this ILocalStorageService localStorage, CultureInfo cultureInfo)
    {
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        await localStorage.SetItemAsync(CURRENT_CULTURE, cultureInfo.Name);
    }
}
