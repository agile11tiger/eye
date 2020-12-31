using EyE.Client.Components;
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
        public readonly T ItemEditorModel = new T();
        public readonly ItemAdderViewModel ItemAdderViewModel = new ItemAdderViewModel();
        public readonly SortingViewModel SortingModel = new SortingViewModel();
        public readonly FilterViewModel FilterModel = new FilterViewModel();
        public readonly PaginationViewModel PaginationModel = new PaginationViewModel();
        [Inject] public JsonSerializerOptions Options { get; set; }
        [Inject] public HttpClient Client { get; set; }
        [Inject] public PublicHttpClient PublicClient { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public string PageURI { get; set; }
        public LinkedList<T> DatabaseItems { get; set; }
        public LinkedList<T> TempItems { get; set; }
        public T RefEditableItem { get; set; }
        public bool IsShowItemEditorWrapper { get; set; }

        public async Task InitializeAsync(string pageURI, bool needUpdateTempItems = true)
        {
            PageURI = pageURI;

            //Получаем список с базы данных ОДИН РАЗ
            DatabaseItems = await PublicClient.GetFromJsonAsync<LinkedList<T>>(PageURI);

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

        public virtual async Task CreateItemAsync()
        {
            if (!await CheckAdminRoleAsync() || !await CheckItemAdderViewModelAsync())
                return;

            var response = await Client.PutAsJsonAsync(PageURI + "/Put", ItemAdderViewModel);
            await TryHandleItemCreationResponseAsync(response);
        }

        public async Task PutItemAsync(T model)
        {
            if (!await CheckAdminRoleAsync())
                return;

            var response = await Client.PutAsJsonAsync(PageURI + "/Put", model);
            await TryHandleItemCreationResponseAsync(response);
        }

        public async Task<bool> TryAddItemAsync(T model)
        {
            if (!await CheckAdminRoleAsync())
                return false;

            var response = await Client.PostAsJsonAsync(PageURI, model);
            return await TryHandleItemCreationResponseAsync(response);
        }

        public async Task<bool> TryHandleItemCreationResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == true)
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
                await ShowErrorAlertAsync(response.StatusCode, responseMessage ?? "Не получилось добавить");
                return false;
            }
        }

        public async Task DeleteItemAsync(int id)
        {
            if (!await CheckAdminRoleAsync())
                return;

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{PageURI}/{id}");
            var response = await Client.SendAsync(request);

            if (response.IsSuccessStatusCode == true)
            {
                var tempItem = new T() { Id = id };
                DatabaseItems.Remove(tempItem);
                TempItems.Remove(tempItem);
                TableHasChanged();
            }
            else
            {
                var responseMessage = await response.Content.ReadAsStringAsync();
                await ShowErrorAlertAsync(response.StatusCode, responseMessage ?? "Не получилось удалить");
            }
        }

        public async Task<bool> TryUpdateItemAsync()
        {
            if (!await CheckAdminRoleAsync())
                return false;

            var response = await Client.PutAsJsonAsync(PageURI, ItemEditorModel);

            if (response.IsSuccessStatusCode == true)
            {
                //копируем свойства из редактора в ссылку на редактируемый объект
                ItemEditorModel.CopyProperties(RefEditableItem);
                IsShowItemEditorWrapper = false;
                return true;
            }

            var responseMessage = await response.Content.ReadAsStringAsync();
            await ShowErrorAlertAsync(response.StatusCode, responseMessage ?? "Не получилось редактировать");
            return false;
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
            await JSRuntime.InvokeVoidAsync("window.open", link, "_blank");
        }

        public async Task<bool> CheckItemAdderViewModelAsync()
        {
            if (!string.IsNullOrWhiteSpace(ItemAdderViewModel.Id))
                return true;
            else
            {
                await ShowErrorAlertNotAllowNullOrWhiteSpaceAsync();
                return false;
            }
        }

        public async Task<bool> CheckAdminRoleAsync()
        {
            if (AuthenticationStateTask.Result.User.IsInRole(Roles.Admin.ToString()))
                return true;
            else
            {
                await ShowErrorAlertAllowOnlyAdminAsync();
                return false;
            }
        }

        public async Task ShowErrorAlertNotAllowNullOrWhiteSpaceAsync()
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Поле должно быть заполнено");
        }

        public async Task ShowErrorAlertAllowOnlyAdminAsync()
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Это действие разрешено только администратору");
        }

        private async Task ShowErrorAlertAsync(HttpStatusCode statusCode, string text)
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Упссс, ошибка {statusCode}! {text}");
        }
    }
}
