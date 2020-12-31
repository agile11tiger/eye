using EyE.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace EyE.Shared.ViewModels.Identity
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(IdentityResource))]
        [EmailAddress(ErrorMessageResourceName = "EmailIncorrect", ErrorMessageResourceType = typeof(IdentityResource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(IdentityResource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = true;
    }
}
