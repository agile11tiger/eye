using System.Globalization;

namespace EyEServer.Resources.Localization;

public static class LocalizationExtension
{
    public static string Format(this string str, params object[] parameters)
    {
        return string.Format(CultureInfo.InvariantCulture, str, parameters);
    }
}

