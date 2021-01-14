using Blazored.LocalStorage;
using EyE.Shared.Helpers;
using EyE.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EyE.Client.Pages
{
    [Route("/")]
    [Route("Quotes")]
    public partial class Quotes
    {
        [Inject] public HttpClient Client { get; set; }
        [Inject] public PublicHttpClient PublicClient { get; set; }
        [Inject] public ILocalStorageService  LocalStorage { get; set; }
        private WikiquoteViewModel WikiquoteViewModel;
        private readonly ItemAdderViewModel ItemAdderViewModel = new ItemAdderViewModel();

        protected override async Task OnInitializedAsync()
        {
            WikiquoteViewModel = await LocalStorage.GetItemAsync<WikiquoteViewModel>("quote");

            if (WikiquoteViewModel == default)
                await GetQuotesAsync();

            await base.OnInitializedAsync();
        }

        public async Task GetQuotesAsync()
        {
            var counter = 0;
            while (counter < 3)
            {
                WikiquoteViewModel = await WikiquoteHelper.GetWikiquoteModelAsync(ItemAdderViewModel?.Id, PublicClient);

                if (WikiquoteViewModel != default)
                    break;

                counter++;
            }

            await LocalStorage.SetItemAsync("quote", WikiquoteViewModel);
        }
    }
}
