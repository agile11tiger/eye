using EyE.Shared.Enums;
using EyE.Shared.Extensions;
using EyE.Shared.Helpers;
using EyE.Shared.Models.Common.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EyE.Client.Pages.Common
{
    public class Folders<T> : Scroll<T> where T : class, IDbFolderItem, new()
    {
        [Parameter] public string StrFolderName { get; set; }
        public FolderNames FolderName { get; set; }
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
                var authState = await UserChecker.GetAuthenticationStateAsync();

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
}
