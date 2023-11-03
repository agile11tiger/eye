using MemoryClient.Components;
using MemoryClient.Services;
using Identity.Models;
using Memory.Extensions;
using Memory.ViewModels;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
namespace MemoryClient.Pages.Common;

public class Scroll<T> : ComponentBase where T : class, IDatabaseItem, new()
{
    public T ItemEditorModel { get; } = new();
    public FilterViewModel FilterModel { get; } = new();
    public SortingViewModel SortingModel { get; } = new();
    public PaginationViewModel PaginationModel { get; } = new();
    public ItemAdderViewModel ItemAdderViewModel { get; } = new();
    [Inject] public UserChecker UserChecker { get; set; }
    [Inject] public ServerHttpClient ServerHttpClient { get; set; }
    [Inject] public PublicHttpClient PublicHttpClient { get; set; }
    [Inject] public JsonSerializerOptions JsonSerializerOptions { get; set; }
    [Inject] public ServerAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    public string PageURI { get; set; }
    public T RefEditableItem { get; set; }
    public LinkedList<T> TempItems { get; set; }
    public LinkedList<T> DatabaseItems { get; set; }
    public bool IsShowItemEditorWrapper { get; set; }

    public virtual async Task InitializeAsync(string pageURI, bool needUpdateTempItems = true)
    {
        PageURI = pageURI;
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

        //Получаем список с базы данных ОДИН РАЗ
        //DatabaseItems = authState.User.Identity.IsAuthenticated
        //    ? await ServerHttpClient.GetFromJsonAsync<LinkedList<T>>(PageURI)
        //    : await PublicHttpClient.GetFromJsonAsync<LinkedList<T>>(PageURI);
        DatabaseItems = await ServerHttpClient.GetFromJsonAsync<LinkedList<T>>(PageURI);
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

        var response = await ServerHttpClient.PutAsJsonAsync(PageURI + "/AddIfNotExist", ItemAdderViewModel);
        await TryHandleItemCreationResponseAsync(response);
    }

    public async Task AddItemIfNotExistAsync(T model)
    {
        if (!await UserChecker.CheckAdminRoleAsync())
            return;

        var response = await ServerHttpClient.PutAsJsonAsync(PageURI + "/AddIfNotExist", model);
        await TryHandleItemCreationResponseAsync(response);
    }

    public async Task<bool> TryAddItemAsync(T model)
    {
        if (!await UserChecker.CheckAdminRoleAsync())
            return false;

        var response = await ServerHttpClient.PostAsJsonAsync(PageURI, model);
        return await TryHandleItemCreationResponseAsync(response);
    }

    public async Task<bool> TryHandleItemCreationResponseAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            using var stream = await response.Content.ReadAsStreamAsync();
            var item = await JsonSerializer.DeserializeAsync<T>(stream, JsonSerializerOptions);
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
        var response = await ServerHttpClient.SendAsync(request);

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

        var response = await ServerHttpClient.PutAsJsonAsync(PageURI, ItemEditorModel);

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

        var response = await ServerHttpClient.PutAsJsonAsync(PageURI, newItem);

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
