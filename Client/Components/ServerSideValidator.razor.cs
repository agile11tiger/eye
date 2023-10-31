using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
namespace EyE.Client.Components;

//https://remibou.github.io/Using-the-Blazor-form-validation/
public partial class ServerSideValidator
{
    private ValidationMessageStore messageStore;
    [CascadingParameter] public EditContext CurrentEditContext { get; set; }

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{nameof(ServerSideValidator)} requires a cascading " +
                $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(ServerSideValidator)} " +
                $"inside an {nameof(EditForm)}.");
        }

        messageStore = new ValidationMessageStore(CurrentEditContext);
        CurrentEditContext.OnValidationRequested += (s, e) => messageStore.Clear();
        CurrentEditContext.OnFieldChanged += (s, e) => messageStore.Clear(e.FieldIdentifier);
    }

    public async Task DisplayMessagesAsync(HttpContent content)
    {
        try
        {
            var messages = await content.ReadFromJsonAsync<Dictionary<string, List<string>>>();

            foreach (var message in messages)
                messageStore.Add(CurrentEditContext.Field(message.Key), message.Value);

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
        messageStore.Add(CurrentEditContext.Field(key), message);
        CurrentEditContext.NotifyValidationStateChanged();
    }
}
