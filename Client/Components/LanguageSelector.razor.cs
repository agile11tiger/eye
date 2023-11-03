using Blazored.LocalStorage;
using MemoryClient.Extensions;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
namespace MemoryClient.Components;

public partial class LanguageSelector
{
    private readonly List<LanguageData> _languagesData =
    [
        new LanguageData("English", "images/usFlag.png", new CultureInfo("en-US")),
        new LanguageData("Русский", "images/ruFlag.png", new CultureInfo("ru-RU")),
    ];
    [Inject] ILocalStorageService Localstorage { get; set; }
    public CultureInfo CurrentCulture
    {
        get => CultureInfo.CurrentCulture;
        set
        {
            if (CultureInfo.CurrentCulture != value)
                Localstorage.SetCultureAsync(value);
        }
    }

    private class LanguageData(string name, string imagePath, CultureInfo cultureInfo)
    {
        public string Name { get; set; } = name;
        public string ImagePath { get; set; } = imagePath;
        public CultureInfo CultureInfo { get; set; } = cultureInfo;
    }
}