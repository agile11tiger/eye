using EyE.Client.Enums;
using EyE.Shared.Extensions;
using EyE.Shared.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EyE.Client.Pages.Common
{
    public class Links : Folders<LinkModel>
    {
        public readonly IDictionary<SortingKeys, string> SortingParameters =
            new Dictionary<SortingKeys, string>()
        {
            { SortingKeys.Name, SortingKeys.Name.GetAttribute<DisplayAttribute>().Name },
        };

        public readonly IDictionary<FilterKeys, string> FilterParameters =
            new Dictionary<FilterKeys, string>()
        {
            { FilterKeys.StartWith, FilterKeys.StartWith.GetAttribute<DisplayAttribute>().Name },
            { FilterKeys.Contains, FilterKeys.Contains.GetAttribute<DisplayAttribute>().Name },
        };

        protected override async Task OnInitializedAsync()
        {
            await InitializeAsync("api/Links", false);
        }

        public override async Task CreateItemAsync()
        {
            if (!await CheckAdminRoleAsync() || !await CheckItemAdderViewModelAsync())
                return;

            var linkModel = new LinkModel()
            {
                Link = ItemAdderViewModel.Id,
                FolderName = FolderName
            };
            await PutItemAsync(linkModel);
        }
    }
}
