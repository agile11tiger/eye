namespace Identity.Models;

public abstract class IdentityResponseModel : ResponseModel
{
    public string? UserId { get; set; }
}
