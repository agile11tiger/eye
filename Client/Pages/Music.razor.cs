using EyE.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace EyE.Client.Pages
{
    [Route("/Music/{StrFolderName}")]
    public partial class Music
    {
        protected override async Task OnInitializedAsync()
        {
            await InitializeAsync("api/Music");
        }

        public override async Task CreateItemAsync()
        {
            if (!await CheckAdminRoleAsync() || !await CheckItemAdderViewModelAsync())
                return;

            var musicModel = await DiscogsHelper.GetMusicModelAsync(ItemAdderViewModel.Id, PublicClient);
            musicModel.FolderName = FolderName;
            await PutItemAsync(musicModel);
        }
    }
}
