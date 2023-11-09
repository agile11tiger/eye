namespace IdentityLib.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(IdentityResource))]
    public string? Email { get; set; }
    //[Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(IdentityResource))]
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = true;
}
