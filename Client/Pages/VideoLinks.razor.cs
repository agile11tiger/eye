namespace EyE.Client.Pages;

[Route("VideoLinks/{StrFolderName}")]
public partial class VideoLinks
{
    public override async Task AddItemIfNotExistAsync()
    {
        if (!await UserChecker.CheckAdminRoleAsync() || !await UserChecker.CheckNullOrWhiteSpaceAsync(ItemAdderViewModel.Id))
            return;

        var linkModel = await YoutubeHelper.GetLinkModelAsync(ItemAdderViewModel.Id, PublicHttpClient);

        if (linkModel == default)
        {
            await UserChecker.ShowSomethingHappenedAsync();
            return;
        }

        linkModel.FolderName = FolderName;
        await AddItemIfNotExistAsync(linkModel);
    }
}
