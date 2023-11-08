using Blazored.LocalStorage;
using Identity.Models;
using Identity.ViewModels;
using MemoryClient.Extensions;
using MemoryClient.Services;
using Microsoft.JSInterop;
using System.Net.Http.Json;
namespace MemoryClient.Components;

public partial class AuthorizationAndRegistration
{
    private bool _isShowWrapper;
    private ElementReference _auth;
    private ElementReference _resetPassword;
    private LoginViewModel _loginModel = new();
    private RegisterViewModel _registerModel = new();
    private ResetPasswordViewModel _resetPasswordModel = new();
    private ForgotPasswordViewModel _forgotPasswordModel = new();
    private ServerSideValidator _serverSideRegistrationValidator = new();
    private ServerSideValidator _serverSideResetPasswordValidator = new();
    private ServerSideValidator _serverSideAuthorizationValidator = new();
    private ServerSideValidator _serverSideForgotPasswordValidator = new();
    [Parameter] public Func<Task> AuthorizationCompleteAsync { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public NavigationManager Navigation { get; set; }
    [Inject] public ILocalStorageService LocalStorage { get; set; }
    [Inject] public ServerHttpClient ServerHttpClient { get; set; }
    [Inject] public ServerAuthenticationStateProvider AuthenticationStateProvider { get; set; }

    private async Task LoginToAccount()
    {
        var response = await ServerHttpClient.PostAsJsonAsync("api/account/login", _loginModel);

        if (response.IsSuccessStatusCode)
        {
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseModel>();
            var userInfo = new UserInfo() { Nickname = loginResponse.Nickname, Token = loginResponse.Token, RefreshToken = loginResponse.RefreshToken };
            await AuthenticationStateProvider.NotifyUserAuthenticationAsync(userInfo);
            ToggleVisibilityWrapper();
            _loginModel = new LoginViewModel();
            await AuthorizationCompleteAsync();
        }
        else
            await _serverSideAuthorizationValidator.DisplayMessagesAsync<ResponseModel>(response.Content);
    }

    private async Task CreateAccount()
    {
        var response = await ServerHttpClient.PostAsJsonAsync("api/account/register", _registerModel);

        if (response.IsSuccessStatusCode)
        {
            _loginModel.Email = _registerModel.Email;
            _loginModel.Password = _registerModel.Password;
            await _serverSideAuthorizationValidator.DisplayMessagesAsync<RegisterResponseModel>(response.Content);
            await _auth.Click(JS);
            _registerModel = new RegisterViewModel();
            StateHasChanged();
        }
        else
            await _serverSideRegistrationValidator.DisplayMessagesAsync<RegisterResponseModel>(response.Content);
    }

    private async Task ForgotPassword()
    {
        var response = await ServerHttpClient.PostAsJsonAsync("api/account/forgotPassword", _forgotPasswordModel);

        if (response.IsSuccessStatusCode)
        {
            _resetPasswordModel.Email = _forgotPasswordModel.Email;
            _forgotPasswordModel = new ForgotPasswordViewModel();
            await _resetPassword.Click(JS);
            StateHasChanged();
        }
        else
            await _serverSideForgotPasswordValidator.DisplayMessagesAsync<RegisterResponseModel>(response.Content);
    }

    private async Task ResetPassword()
    {
        var response = await ServerHttpClient.PostAsJsonAsync("api/account/resetPassword", _resetPasswordModel);

        if (response.IsSuccessStatusCode == true)
        {
            _resetPasswordModel = new ResetPasswordViewModel();
            ToggleVisibilityWrapper();
        }
        else
            await _serverSideResetPasswordValidator.DisplayMessagesAsync<ResponseModel>(response.Content);
    }

    private void ToggleVisibilityWrapper()
    {
        _isShowWrapper = !_isShowWrapper;
    }
}
