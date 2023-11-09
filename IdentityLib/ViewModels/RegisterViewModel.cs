namespace IdentityLib.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessageResourceName = "NicknameRequired", ErrorMessageResourceType = typeof(IdentityResource))]
    [RegularExpression(@"[^@]+", ErrorMessageResourceName = "NicknameShouldNotContain", ErrorMessageResourceType = typeof(IdentityResource))]
    public string? Nickname { get; set; }
    [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(IdentityResource))]
    [EmailAddress(ErrorMessageResourceName = "EmailIncorrect", ErrorMessageResourceType = typeof(IdentityResource))]
    public string? Email { get; set; }

    //[Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(IdentityResource))]
    //[StringLength(15, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(IdentityResource), MinimumLength = 6)]
    //[RegularExpression(
    //    @"^(?=.*[A-ZА-Я])(?=.*[a-zа-я])(?=.*\d).*$",
    //    ErrorMessageResourceName = "PasswordOneUppercaseAndOneLowercaseAndOneNumeric",
    //    ErrorMessageResourceType = typeof(IdentityResource))]
    public string Password { get; set; } = string.Empty;

    //[Required(ErrorMessageResourceName = "PasswordConfirmRequired", ErrorMessageResourceType = typeof(IdentityResource))]
    //[Compare("Password", ErrorMessageResourceName = "PasswordsNotMatch", ErrorMessageResourceType = typeof(IdentityResource))]
    //[DataType(DataType.Password)]
    //public string? PasswordConfirm { get; set; }

    //[Range(typeof(bool), "true", "true", ErrorMessageResourceName = "RegistrationConditions", ErrorMessageResourceType = typeof(IdentityResource))]
    //public bool AcceptedRegistrationConditions { get; set; }
}
