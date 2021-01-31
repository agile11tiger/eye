using EyE.Client.Services;
using EyE.Shared.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyE.Client.Components
{
    public partial class AdminMenu
    {
        [Inject] public HttpClient Client { get; set; }
        [Inject] public UserChecker UserChecker { get; set; }

        public async Task UpdateDiscogsImageSourcesAsync()
        {
            if (await UserChecker.CheckAdminRoleAsync())
            {
                await Client.PostAsync("api/Music/UpdateImageSources", null);
                await UserChecker.JS.InvokeVoidAsync("alert", $"Обновление изображений завершено");
            }
        }

        public async Task UpdateFilmsAsync()
        {
            if (await UserChecker.CheckAdminRoleAsync())
            {
                await Client.PostAsync("api/Films/UpdateItems", null);
                await UserChecker.JS.InvokeVoidAsync("alert", $"Обновление фильмов завершено");
            }
        }

        public async Task UpdateSerialssAsync()
        {
            if (await UserChecker.CheckAdminRoleAsync())
            {
                await Client.PostAsync("api/Serials/UpdateItems", null);
                await UserChecker.JS.InvokeVoidAsync("alert", $"Обновление сериалов завершено");
            }
        }

        public async Task UpdateGamesItemsAsync()
        {
            if (await UserChecker.CheckAdminRoleAsync())
            {
                await Client.PostAsync("api/Games/UpdateItems", null);
                await UserChecker.JS.InvokeVoidAsync("alert", $"Обновление игр завершено");
            }
        }
    }
}
