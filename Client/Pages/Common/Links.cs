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
        public readonly Dictionary<SortingKeys, string> SortingParameters = new()
        {
            { SortingKeys.Name, SortingKeys.Name.GetAttribute<DisplayAttribute>().Name },
        };

        public readonly Dictionary<FilterKeys, string> FilterParameters = new()
        {
            { FilterKeys.StartWith, FilterKeys.StartWith.GetAttribute<DisplayAttribute>().Name },
            { FilterKeys.Contains, FilterKeys.Contains.GetAttribute<DisplayAttribute>().Name },
        };

        protected override async Task OnInitializedAsync()
        {
            await InitializeAsync("api/Links", false);
        }

        public override async Task AddItemIfNotExistAsync()
        {
            if (!await UserChecker.CheckAdminRoleAsync() || !await UserChecker.CheckNullOrWhiteSpaceAsync(ItemAdderViewModel.Id))
                return;

            var linkModel = new LinkModel()
            {
                Link = ItemAdderViewModel.Id,
                FolderName = FolderName
            };
            await AddItemIfNotExistAsync(linkModel);
        }
    }
}
