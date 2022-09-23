using EyE.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace EyE.Shared.ViewModels.Identity
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(IdentityResource))]
        [EmailAddress(ErrorMessageResourceName = "EmailIncorrect", ErrorMessageResourceType = typeof(IdentityResource))]
        public string? Email { get; set; }
    }
}
