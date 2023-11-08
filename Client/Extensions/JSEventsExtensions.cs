using Microsoft.JSInterop;
namespace MemoryClient.Extensions;

public static class JSEventsExtensions
{
    public static async Task Click(this ElementReference elementRef, IJSRuntime js)
    {
        await js.InvokeVoidAsync("events.clickElement", elementRef);
    }
}
