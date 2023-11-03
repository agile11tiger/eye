using Identity.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
namespace MemoryClient.Components;

/// <summary>
/// https://remibou.github.io/Using-the-Blazor-form-validation/
/// </summary>
public partial class ServerSideValidator
{
    private ValidationMessageStore _messageStore;
    [CascadingParameter] public EditContext CurrentEditContext { get; set; }

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{nameof(ServerSideValidator)} requires a cascading " +
                $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(ServerSideValidator)} " +
                $"inside an {nameof(EditForm)}.");
        }

        _messageStore = new ValidationMessageStore(CurrentEditContext);
        CurrentEditContext.OnValidationRequested += (s, e) => _messageStore.Clear();
        CurrentEditContext.OnFieldChanged += (s, e) => _messageStore.Clear(e.FieldIdentifier);
    }

    public async Task DisplayMessagesAsync<T>(HttpContent content) where T: ResponseModel
    {
        try
        {
            var identityModel = await content.ReadFromJsonAsync<T>();

            foreach (var message in identityModel.Messages)
                _messageStore.Add(CurrentEditContext.Field(string.Empty), message);

            CurrentEditContext.NotifyValidationStateChanged();
        }
        catch
        {
            await DisplayMessageAsync(content);
        }

    }

    public async Task DisplayMessageAsync(HttpContent content, string key = "")
    {
        var message = await content.ReadAsStringAsync();
        _messageStore.Add(CurrentEditContext.Field(key), message);
        CurrentEditContext.NotifyValidationStateChanged();
    }
}
