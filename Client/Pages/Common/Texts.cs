using EyE.Client.Enums;
using EyE.Shared.Enums;
using EyE.Shared.Extensions;
using EyE.Shared.Models.Common;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EyE.Client.Pages.Common
{
    public class Texts : Folders<TextModel>
    {
        public readonly IDictionary<SortingKeys, string> SortingParametersDictionary =
            new Dictionary<SortingKeys, string>()
        {
            { SortingKeys.Text, "Текста" },
        };
        public readonly IDictionary<FilterKeys, string> FilterParametersDictionary =
            new Dictionary<FilterKeys, string>()
        {
            { FilterKeys.StartWith, "Начинаеться с" },
            { FilterKeys.Contains, "Содержит" },
        };
        public TextModel NewTextModel = new TextModel();

        protected override async Task OnInitializedAsync()
        {
            SortingModel.CurrentSortingParameter = SortingKeys.Text;
            await InitializeAsync("api/Texts", false);
        }

        public override async Task CreateItemAsync()
        {
            if (!await CheckAdminRoleAsync())
                return;

            if (string.IsNullOrWhiteSpace(NewTextModel.Text))
            {
                await ShowErrorAlertNotAllowNullOrWhiteSpaceAsync();
                return;
            }

            NewTextModel.FolderName = FolderName;

            if (await TryAddItemAsync(NewTextModel))
                NewTextModel = new TextModel();
        }

        public override void ShowItemEditor(object objItem)
        {
            if (RefEditableItem == default || (RefEditableItem != default && !RefEditableItem.IsEditing))
            {
                base.ShowItemEditor(objItem);
                RefEditableItem.IsEditing = true;
            }
        }

        public async Task CloseItemEditorAsync()
        {
            if (RefEditableItem != default && RefEditableItem.IsEditing && await TryUpdateItemAsync())
                RefEditableItem.IsEditing = false;
        }
    }
}
