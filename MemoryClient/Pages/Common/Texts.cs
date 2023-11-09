using MemoryLib.Models.Common;
using MemoryClient.Enums;
using System.Collections.Generic;
namespace MemoryClient.Pages.Common;

public class Texts : Folders<TextModel>
{
    public TextModel NewTextModel { get; private set; } = new();
    public readonly Dictionary<SortingKeys, string> SortingParametersDictionary = new()
    {
        { SortingKeys.Text, "Текста" },
    };
    public readonly Dictionary<FilterKeys, string> FilterParametersDictionary = new()
    {
        { FilterKeys.StartWith, "Начинается с" },
        { FilterKeys.Contains, "Содержит" },
    };

    protected override async Task OnInitializedAsync()
    {
        SortingModel.CurrentSortingParameter = SortingKeys.Text;
        await InitializeAsync("api/Texts", false);
    }

    public override async Task AddItemIfNotExistAsync()
    {
        if (!await UserChecker.CheckAdminRoleAsync())
            return;

        if (string.IsNullOrWhiteSpace(NewTextModel.Text))
        {
            await UserChecker.ShowErrorAlertNotAllowNullOrWhiteSpaceAsync();
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
