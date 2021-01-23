using EyE.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace EyE.Client.Pages
{
    [Route("VideoLinks/{StrFolderName}")]
    public partial class VideoLinks
    {
        public override async Task CreateItemAsync()
        {
            if (!await UserChecker.CheckAdminRoleAsync() || !await UserChecker.CheckNullOrWhiteSpaceAsync(ItemAdderViewModel.Id))
                return;

            var linkModel = await YoutubeHelper.GetLinkModelAsync(ItemAdderViewModel.Id, PublicClient);

            if (linkModel == default)
            {
                await UserChecker.ShowSomethingHappenedAsync();
                return;
            }

            linkModel.FolderName = FolderName;
            await PutItemAsync(linkModel);
        }
    }
}
