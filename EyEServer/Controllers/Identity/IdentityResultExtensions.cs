using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
namespace EyEServer.Controllers.Identity;

public static class IdentityResultExtensions
{
    public static IEnumerable<string> GetMessages(this IdentityResult result)
    {
        return result.Errors.Select(e => $"{e.Code}: {e.Description}");
    }
}
