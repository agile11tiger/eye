using EyE.Client.Components;
using EyE.Client.Services;
using EyE.Shared.Enums;
using EyE.Shared.Extensions;
using EyE.Shared.Models.Common.Interfaces;
using EyE.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace EyE.Client.Pages.Common
{
    public class Scroll<T> : ComponentBase where T : class, IDatabaseItem, new()
    {
        public readonly T ItemEditorModel = new();
        public readonly ItemAdderViewModel ItemAdderViewModel = new();
        public readonly SortingViewModel SortingModel = new();
        public readonly FilterViewModel FilterModel = new();
        public readonly PaginationViewModel PaginationModel = new();
        [Inject] public JsonSerializerOptions Options { get; set; }
        [Inject] public HttpClient Client { get; set; }
        [Inject] public PublicHttpClient PublicClient { get; set; }
        [Inject] public UserChecker UserChecker { get; set; }

        public string PageURI { get; set; }
        public LinkedList<T> DatabaseItems { get; set; }
        public LinkedList<T> TempItems { get; set; }
        public T RefEditableItem { get; set; }
        public bool IsShowItemEditorWrapper { get; set; }

        public virtual async Task InitializeAsync(string pageURI, bool needUpdateTempItems = true)
        {
            PageURI = pageURI;
            var authState = await UserChecker.GetAuthenticationStateAsync();

            //Получаем список с базы данных ОДИН РАЗ
            DatabaseItems = authState.User.Identity.IsAuthenticated
                ? await Client.GetFromJsonAsync<LinkedList<T>>(PageURI)
                : await PublicClient.GetFromJsonAsync<LinkedList<T>>(PageURI);

            if (needUpdateTempItems)
                UpdateTempItems();
        }

        /// <param name="items">object should be T</param>
        public void UpdateTempItems(IEnumerable<object> items = null)
        {
            TempItems = new LinkedList<T>(items?.Select(item => (T)item) ?? DatabaseItems);
            PaginationModel.PageNumber = PaginationModel.PageCountStart;
            TableHasChanged();
        }

        public virtual async Task AddItemIfNotExistAsync()
        {
            if (!await UserChecker.CheckAdminRoleAsync() || !await UserChecker.CheckNullOrWhiteSpaceAsync(ItemAdderViewModel.Id))
                return;

            var response = await Client.PutAsJsonAsync(PageURI + "/AddIfNotExist", ItemAdderViewModel);
            await TryHandleItemCreationResponseAsync(response);
        }

        public async Task AddItemIfNotExistAsync(T model)
        {
            if (!await UserChecker.CheckAdminRoleAsync())
                return;

            var response = await Client.PutAsJsonAsync(PageURI + "/AddIfNotExist", model);
            await TryHandleItemCreationResponseAsync(response);
        }

        public async Task<bool> TryAddItemAsync(T model)
        {
            if (!await UserChecker.CheckAdminRoleAsync())
                return false;

            var response = await Client.PostAsJsonAsync(PageURI, model);
            return await TryHandleItemCreationResponseAsync(response);
        }

        public async Task<bool> TryHandleItemCreationResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                var item = await JsonSerializer.DeserializeAsync<T>(stream, Options);
                DatabaseItems.AddFirst(item);
                TempItems.AddFirst(item);
                TableHasChanged();
                ItemAdderViewModel.Id = default;
                return true;
            }
            else
            {
                var responseMessage = await response.Content.ReadAsStringAsync();
                await UserChecker.ShowErrorAlertAsync(response.StatusCode, responseMessage ?? "Не получилось добавить");
                return false;
            }
        }

        public async Task DeleteItemAsync(int id)
        {
            if (!await UserChecker.CheckAdminRoleAsync())
                return;

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{PageURI}/{id}");
            var response = await Client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var tempItem = new T() { Id = id };
                DatabaseItems.Remove(tempItem);
                TempItems.Remove(tempItem);
                TableHasChanged();
            }
            else
            {
                var responseMessage = await response.Content.ReadAsStringAsync();
                await UserChecker.ShowErrorAlertAsync(response.StatusCode, responseMessage ?? "Не получилось удалить");
            }
        }

        /// <summary>
        /// Берёт данные из модели редактора и пытается обновиться
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TryUpdateItemAsync()
        {
            if (!await UserChecker.CheckAdminRoleAsync())
                return false;

            var response = await Client.PutAsJsonAsync(PageURI, ItemEditorModel);

            if (response.IsSuccessStatusCode)
            {
                //копируем свойства из редактора в ссылку на редактируемый объект
                ItemEditorModel.CopyProperties(RefEditableItem);
                IsShowItemEditorWrapper = false;
                return true;
            }

            var responseMessage = await response.Content.ReadAsStringAsync();
            await UserChecker.ShowErrorAlertAsync(response.StatusCode, responseMessage ?? "Не получилось редактировать");
            return false;
        }

        public virtual async Task UpdateItemAsync(T oldItem, T newItem)
        {
            if (!await UserChecker.CheckAdminRoleAsync())
                return;

            var response = await Client.PutAsJsonAsync(PageURI, newItem);

            if (!response.IsSuccessStatusCode)
            {
                var responseMessage = await response.Content.ReadAsStringAsync();
                await UserChecker.ShowErrorAlertAsync(response.StatusCode, responseMessage ?? "Не получилось обновить");
            }
            else
                newItem.CopyProperties(oldItem);
        }

        public virtual void ShowItemEditor(object objItem)
        {
            //objItem это cсылка на объект, который есть в DatabaseItems, tempItems
            if (objItem is T item)
            {
                IsShowItemEditorWrapper = true;
                //копируем свойства в редактор
                item.CopyProperties(ItemEditorModel);
                //запоминаем ссылку на редактируемый объект
                RefEditableItem = item;
            }
        }

        public void TableHasChanged()
        {
            PaginationModel.Count = TempItems.Count;
        }

        public async Task OpenLinkInNewTabAsync(string link)
        {
            await UserChecker.JS.InvokeVoidAsync("window.open", link, "_blank");
        }
    }
}
