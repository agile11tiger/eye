using EyE.Client.Enums;
using EyE.Shared.Enums;
using EyE.Shared.Extensions;
using EyE.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EyE.Client.Pages
{
    [Route("/Films/{StrFolderName}")]
    public partial class Films
    {
        [Inject] public IAccessTokenProvider TokenProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            SortingParameters.Add(SortingKeys.IMDbRating, SortingKeys.IMDbRating.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.IMDbRating, FilterKeys.IMDbRating.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.IMDbRatingMore, FilterKeys.IMDbRatingMore.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.IMDbRatingLess, FilterKeys.IMDbRatingLess.GetAttribute<DisplayAttribute>().Name);

            await InitializeAsync("api/Films");
        }

        public override async Task CreateItemAsync()
        {
            if (!await CheckAdminRoleAsync() || !await CheckItemAdderViewModelAsync())
                return;

            var filmModel = await IMDbHelper.GetFilmModelAsync(ItemAdderViewModel.Id, PublicClient);
            filmModel.FolderName = FolderName;
            await PutItemAsync(filmModel);
        }
    }
}
