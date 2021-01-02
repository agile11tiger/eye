using EyE.Client.Enums;
using EyE.Shared.Extensions;
using EyE.Shared.Helpers;
using EyE.Shared.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EyE.Client.Pages
{
    [Route("Anime/{StrFolderName}")]
    public partial class Anime
    {
        protected override async Task OnInitializedAsync()
        {
            SortingParameters.Add(SortingKeys.AniDbRating, SortingKeys.AniDbRating.GetAttribute<DisplayAttribute>().Name);
            SortingParameters.Add(SortingKeys.MyRating, SortingKeys.MyRating.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.AniDbRating, FilterKeys.AniDbRating.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.AniDbRatingMore, FilterKeys.AniDbRatingMore.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.AniDbRatingLess, FilterKeys.AniDbRatingLess.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.MyRating, FilterKeys.MyRating.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.MyRatingMore, FilterKeys.MyRatingMore.GetAttribute<DisplayAttribute>().Name);
            FilterParameters.Add(FilterKeys.MyRatingLess, FilterKeys.MyRatingLess.GetAttribute<DisplayAttribute>().Name);

            await InitializeAsync("api/Anime");
        }

        public override async Task CreateItemAsync()
        {
            if (!await CheckAdminRoleAsync() || !await CheckItemAdderViewModelAsync())
                return;

            var animemodel = new AnimeModel()
            {
                Link = ItemAdderViewModel.Id,
                AniDbId = AniDbHelper.GetId(ItemAdderViewModel.Id),
                FolderName = FolderName,
            };
            await PutItemAsync(animemodel);
        }
    }
}
