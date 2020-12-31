using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EyE.Server.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddModelErrors(this ModelStateDictionary modelState, IdentityResult result)
        {
            foreach (var error in result.Errors)
                modelState.AddModelError(string.Empty, error.Description);
        }
    }
}
