using Blazored.LocalStorage;
using EyE.Client.Extensions;
using EyE.Client.Services;
using EyE.Shared.Models.Common;
using EyE.Shared.Models.Identity;
using EyE.Shared.ViewModels.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EyE.Client.Components
{
    public partial class AuthorizationAndRegistration
    {
        private bool isShowWrapper;
        private LoginViewModel loginModel = new();
        private RegisterViewModel registerModel = new();
        private ForgotPasswordViewModel forgotPasswordModel = new();
        private ResetPasswordViewModel resetPasswordModel = new();
        private ServerSideValidator serverSideAuthorizationValidator = new();
        private ServerSideValidator serverSideRegistrationValidator = new();
        private ServerSideValidator serverSideForgotPasswordValidator = new();
        private ServerSideValidator serverSideResetPasswordValidator = new();
        [Inject] public ServerHttpClient ServerHttpClient { get; set; }
        [Inject] public ServerAuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] public IJSRuntime JS { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }
        [Inject] public ILocalStorageService LocalStorage { get; set; }

        private void SecretLoginToAccount()
        {
            Navigation.NavigateTo($"authentication/{RemoteAuthenticationActions.LogIn}?returnUrl={Uri.EscapeDataString(Navigation.Uri)}");
        }

        private async Task LoginToAccount()
        {
            var response = await ServerHttpClient.PostAsJsonAsync("api/account/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                loginModel = new LoginViewModel();
                ToogleVisibilityWrapper();
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseModel>();
                var userInfo = new UserInfo() { Token = loginResponse.Token, RefreshToken = loginResponse.RefreshToken };
                await LocalStorage.SetItemAsync(userInfo);
                AuthenticationStateProvider.NotifyUserAuthentication(userInfo.Token);
            }
            else
                await JS.InvokeVoidAsync("alert", "Не пущу без регистрации!");

            await serverSideRegistrationValidator.DisplayMessagesAsync(response.Content);
        }

        private async Task CreateAccount()
        {
            var response = await ServerHttpClient.PostAsJsonAsync("api/account/register", registerModel);

            if (response.IsSuccessStatusCode)
            {
                registerModel = new RegisterViewModel();
                StateHasChanged();
            }

            await serverSideRegistrationValidator.DisplayMessagesAsync(response.Content);
        }

        private async Task ForgotPassword()
        {
            var response = await ServerHttpClient.PostAsJsonAsync("api/account/forgotPassword", forgotPasswordModel);

            if (response.IsSuccessStatusCode)
            {
                resetPasswordModel.Email = forgotPasswordModel.Email;
                forgotPasswordModel = new ForgotPasswordViewModel();
                StateHasChanged();
            }

            await serverSideRegistrationValidator.DisplayMessagesAsync(response.Content);
        }

        private async Task ResetPassword()
        {
            var response = await ServerHttpClient.PostAsJsonAsync("api/account/resetPassword", resetPasswordModel);

            if (response.IsSuccessStatusCode == true)
            {
                resetPasswordModel = new ResetPasswordViewModel();
                ToogleVisibilityWrapper();
            }
            
            await serverSideResetPasswordValidator.DisplayMessagesAsync(response.Content);
        }

        private void ToogleVisibilityWrapper()
        {
            isShowWrapper = !isShowWrapper;
        }
    }
}
