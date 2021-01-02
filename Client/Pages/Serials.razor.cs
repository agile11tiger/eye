using EyE.Client.Enums;
using EyE.Shared.Extensions;
using EyE.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EyE.Client.Pages
{
    [Route("Serials/{StrFolderName}")]
    public partial class Serials
    {
        protected override async Task OnInitializedAsync()
        {
            SortingParameters.Add(SortingKeys.IMDbRating, SortingKeys.IMDbRating.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.IMDbRating, FilterKeys.IMDbRating.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.IMDbRatingMore, FilterKeys.IMDbRatingMore.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.IMDbRatingLess, FilterKeys.IMDbRatingLess.GetAttribute<DisplayAttribute>().Name);

            await InitializeAsync("api/Serials");
        }

        public override async Task CreateItemAsync()
        {
            if (!await CheckAdminRoleAsync() || !await CheckItemAdderViewModelAsync())
                return;

            var serialModel = await IMDbHelper.GetSerialModelAsync(ItemAdderViewModel.Id, PublicClient);
            serialModel.FolderName = FolderName;
            await PutItemAsync(serialModel);
        }
    }
}
