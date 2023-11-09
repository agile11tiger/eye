using IdentityLib.Enums;
using MemoryLib.Enums;
using MemoryLib.Extensions;
using MemoryLib.Models.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace MemoryClient.Pages.Common;

public class Folders<T> : Scroll<T> where T : class, IDbFolderItem, new()
{
    public FolderNames FolderName { get; set; }
    [Parameter] public string StrFolderName { get; set; }
    public string Header
    {
        get => string.IsNullOrEmpty(StrFolderName)
            ? string.Empty
            : FolderName.GetAttribute<DisplayAttribute>().Name;
    }

    protected override async Task OnParametersSetAsync()
    {
        Enum.TryParse(StrFolderName, true, out FolderNames folderName);
        FolderName = folderName;
        Reset();
        await base.OnParametersSetAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender && AdminHelper.AdminFolders.Contains(FolderName))
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            if (!authState.User.IsInRole(Roles.Admin.ToString()))
            {
                await Task.Delay(100);
                await UserChecker.ShowErrorAdminPageAsync();
            }
        }
    }

    public void Reset()
    {
        UpdateTempItems(DatabaseItems.Where(r => r.FolderName == FolderName));
    }

    public async Task UpdateItemAsync()
    {
        //Пытаемся обновить пункт. Проверяем, если он изменил папку назначения, то удаляем.
        if (await TryUpdateItemAsync() && RefEditableItem.FolderName != FolderName)
            TempItems.Remove(RefEditableItem);
    }
}
