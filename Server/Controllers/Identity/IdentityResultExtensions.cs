using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EyE.Server.Controllers.Identity
{
    public static class IdentityResultExtensions
    {
        public static IEnumerable<string> GetMessages(this IdentityResult result)
        {
            return result.Errors.Select(e => $"{e.Code}: {e.Description}");
        }
    }
}
