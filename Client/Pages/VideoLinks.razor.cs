using EyE.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace EyE.Client.Pages
{
    [Route("/VideoLinks/{StrFolderName}")]
    public partial class VideoLinks
    {
        public override async Task CreateItemAsync()
        {
            if (!await CheckAdminRoleAsync() || !await CheckItemAdderViewModelAsync())
                return;

            var linkModel = await YoutubeHelper.GetLinkModelAsync(ItemAdderViewModel.Id, PublicClient);
            linkModel.FolderName = FolderName;
            await PutItemAsync(linkModel);
        }
    }
}
