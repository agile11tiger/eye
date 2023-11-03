using MemoryClient.Enums;
using Memory.Extensions;
using Memory.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace MemoryClient.Pages.Common;

public class IMDbReviews<T> : Reviews<T> where T : IMDbModel, new()
{
    public override async Task InitializeAsync(string pageURI, bool needUpdateTempItems = true)
    {
        SortingParameters.Add(SortingKeys.IMDbRating, SortingKeys.IMDbRating.GetAttribute<DisplayAttribute>().Name);
        FilterParameters.Add(FilterKeys.IMDbRating, FilterKeys.IMDbRating.GetAttribute<DisplayAttribute>().Name);
        FilterParameters.Add(FilterKeys.IMDbRatingMore, FilterKeys.IMDbRatingMore.GetAttribute<DisplayAttribute>().Name);
        FilterParameters.Add(FilterKeys.IMDbRatingLess, FilterKeys.IMDbRatingLess.GetAttribute<DisplayAttribute>().Name);

        await base.InitializeAsync(pageURI);
    }

    public override async Task AddItemIfNotExistAsync()
    {
        if (!await UserChecker.CheckAdminRoleAsync() || !await UserChecker.CheckNullOrWhiteSpaceAsync(ItemAdderViewModel.Id))
            return;

        var model = await IMDbHelper.GetIMDbModelAsync<T>(ItemAdderViewModel.Id, PublicHttpClient);
        model.FolderName = FolderName;
        await AddItemIfNotExistAsync((T)model);
    }

    public async Task UpdateItemAsync(T oldItem)
    {
        if (!await UserChecker.CheckAdminRoleAsync())
            return;

        var newItem = await IMDbHelper.GetIMDbModelAsync<T>(oldItem.Link, PublicHttpClient);
        newItem.Id = oldItem.Id;
        newItem.FolderName = oldItem.FolderName;
        newItem.AddingDate = oldItem.AddingDate;
        await UpdateItemAsync(oldItem, (T)newItem);
    }
}
