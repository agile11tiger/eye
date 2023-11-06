namespace Identity.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(IdentityResource))]
    public string? Email { get; set; }

    [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(IdentityResource))]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public bool RememberMe { get; set; } = true;
    public string? RedirectUri { get; set; }
}
