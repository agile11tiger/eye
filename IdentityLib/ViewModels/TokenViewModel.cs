﻿namespace IdentityLib.ViewModels;

public class TokenViewModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string Message { get; set; }
}
