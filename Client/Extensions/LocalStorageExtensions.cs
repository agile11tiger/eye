using Blazored.LocalStorage;
using System.Threading;
using System.Threading.Tasks;

namespace EyE.Client.Extensions
{
    public static class LocalStorageExtensions
    {
        public static async Task<T> GetItemAsync<T>(this ILocalStorageService localStorage, CancellationToken? cancellationToken = null)
        {
            return await localStorage.GetItemAsync<T>(typeof(T).Name, cancellationToken);
        }

        public static async Task SetItemAsync<T>(this ILocalStorageService localStorage, T data, CancellationToken? cancellationToken = null)
        {
            await localStorage.SetItemAsync(typeof(T).Name, data, cancellationToken);
        }
    }
}
