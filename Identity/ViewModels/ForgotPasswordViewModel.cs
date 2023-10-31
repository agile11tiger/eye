namespace Identity.ViewModels;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(IdentityResource))]
    [EmailAddress(ErrorMessageResourceName = "EmailIncorrect", ErrorMessageResourceType = typeof(IdentityResource))]
    public string? Email { get; set; }
}
