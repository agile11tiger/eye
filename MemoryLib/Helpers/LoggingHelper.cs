using System.Net.Http.Json;
using System.Runtime.CompilerServices;
namespace MemoryLib.Helpers;

public static class LoggingHelper
{
    public static async Task SendErrorAsync(string message, HttpClient client, string className, [CallerMemberName] string? memberName = null)
    {
        await client.PostAsJsonAsync("Logging/AddError", $"{DateTime.Now}\r\n{className}.{memberName}: " + message);
    }
}
