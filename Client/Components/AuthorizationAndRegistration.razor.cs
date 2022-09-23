using EyE.Shared.ViewModels.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using System;
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
        [Inject] public PublicHttpClient PublicClient { get; set; }
        [Inject] public AuthenticationStateProvider StateProvider { get; set; }
        [Inject] public IJSRuntime JS { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }

        private void SecretLoginToAccount()
        {
            Navigation.NavigateTo($"authentication/{RemoteAuthenticationActions.LogIn}?returnUrl={Uri.EscapeDataString(Navigation.Uri)}");
        }

        private async Task LoginToAccount()
        {
            var response = await PublicClient.PostAsJsonAsync("api/account/login", loginModel);

            if (response.IsSuccessStatusCode == true)
            {
                loginModel = new LoginViewModel();
                ToogleVisibilityWrapper();
                //var authService = StateProvider as RemoteAuthenticationService<RemoteAuthenticationState, RemoteUserAccount, ApiAuthorizationProviderOptions>;
                //await authService?.SignInAsync(new RemoteAuthenticationContext<RemoteAuthenticationState>());
                await JS.InvokeVoidAsync("alert", "Всё равно не пущу!!!");
            }
            else
            {
                await serverSideAuthorizationValidator.DisplayMessagesAsync(response.Content);
                await JS.InvokeVoidAsync("alert", "Не пущу без регистрации!");
            }
        }

        private async Task CreateAccount()
        {
            var response = await PublicClient.PostAsJsonAsync("api/account/register", registerModel);

            if (response.IsSuccessStatusCode == true)
            {
                registerModel = new RegisterViewModel();
                StateHasChanged();

                await serverSideRegistrationValidator.DisplayMessageAsync(response.Content);
            }
            else
                await serverSideRegistrationValidator.DisplayMessagesAsync(response.Content);
        }

        private async Task ForgotPassword()
        {
            var response = await PublicClient.PostAsJsonAsync("api/account/forgotPassword", forgotPasswordModel);

            if (response.IsSuccessStatusCode == true)
            {
                resetPasswordModel.Email = forgotPasswordModel.Email;
                forgotPasswordModel = new ForgotPasswordViewModel();
                StateHasChanged();
                //TODO сделать в один вызов метода
                await serverSideForgotPasswordValidator.DisplayMessageAsync(response.Content);
            }
            else
                await serverSideForgotPasswordValidator.DisplayMessagesAsync(response.Content);
        }

        private async Task ResetPassword()
        {
            var response = await PublicClient.PostAsJsonAsync("api/account/resetPassword", resetPasswordModel);

            if (response.IsSuccessStatusCode == true)
            {
                resetPasswordModel = new ResetPasswordViewModel();
                ToogleVisibilityWrapper();
            }
            else
                await serverSideResetPasswordValidator.DisplayMessagesAsync(response.Content);
        }

        private void ToogleVisibilityWrapper()
        {
            isShowWrapper = !isShowWrapper;
        }
    }
}
