using Blazored.LocalStorage;
using System.Threading;
namespace EyE.Client.Extensions;

public static class LocalStorageExtensions
{
    public static async Task<T> GetItemAsync<T>(this ILocalStorageService localStorage, CancellationToken cancellationToken = default)
    {
        return await localStorage.GetItemAsync<T>(typeof(T).Name, cancellationToken);
    }

    public static async Task SetItemAsync<T>(this ILocalStorageService localStorage, T data, CancellationToken cancellationToken = default)
    {
        await localStorage.SetItemAsync(typeof(T).Name, data, cancellationToken);
    }
}
