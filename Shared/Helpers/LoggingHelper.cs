using EyE.Shared.Models.Common;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace EyE.Shared.Helpers
{
    public static class LoggingHelper
    {
        public static async Task SendErrorAsync(string message, HttpClient client, string className, [CallerMemberName] string memberName = null)
        {
            await client.PostAsJsonAsync("Logging/AddError", $"{className}.{memberName}: " + message);
        }
    }
}
