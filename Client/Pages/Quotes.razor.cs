using EyE.Client.Enums;
using EyE.Shared.Helpers;
using EyE.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EyE.Client.Pages
{
    [Route("/")]
    [Route("/Quotes")]
    public partial class Quotes
    {
        [Inject] public PublicHttpClient PublicClient { get; set; }
        private WikiquoteViewModel WikiquoteViewModel;
        private readonly ItemAdderViewModel ItemAdderViewModel = new ItemAdderViewModel();

        protected override async Task OnInitializedAsync()
        {
            await GetQuotesAsync();
            await base.OnInitializedAsync();
        }

        public async Task GetQuotesAsync()
        {
            WikiquoteViewModel = await WikiquoteHelper.GetWikiquoteModelAsync(ItemAdderViewModel?.Id, PublicClient);
        }
    }
}
