using EyE.Shared.ViewModels.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EyE.Client.Components
{
    public partial class AuthorizationAndRegistration
    {
        private bool isShowWrapper;
        private LoginViewModel loginModel = new LoginViewModel();
        private RegisterViewModel registerModel = new RegisterViewModel();
        private ForgotPasswordViewModel forgotPasswordModel = new ForgotPasswordViewModel();
        private ResetPasswordViewModel resetPasswordModel = new ResetPasswordViewModel();
        private ServerSideValidator serverSideAuthorizationValidator = new ServerSideValidator();
        private ServerSideValidator serverSideRegistrationValidator = new ServerSideValidator();
        private ServerSideValidator serverSideForgotPasswordValidator = new ServerSideValidator();
        private ServerSideValidator serverSideResetPasswordValidator = new ServerSideValidator();
        [Inject] public PublicHttpClient PublicClient { get; set; }
        [Inject] public AuthenticationStateProvider StateProvider { get; set; }

        private async Task LoginToAccount()
        {
            var response = await PublicClient.PostAsJsonAsync("/api/account/login", loginModel);

            if (response.IsSuccessStatusCode == true)
            {
                loginModel = new LoginViewModel();
                ToogleVisibilityWrapper();

                var authService = StateProvider as RemoteAuthenticationService<RemoteAuthenticationState, RemoteUserAccount, ApiAuthorizationProviderOptions>;
                await authService?.SignInAsync(new RemoteAuthenticationContext<RemoteAuthenticationState>());
            }
            else
                await serverSideAuthorizationValidator.DisplayMessagesAsync(response.Content);
        }

        private async Task CreateAccount()
        {
            var response = await PublicClient.PostAsJsonAsync("/api/account/register", registerModel);

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
            var response = await PublicClient.PostAsJsonAsync("/api/account/forgotPassword", forgotPasswordModel);

            if (response.IsSuccessStatusCode == true)
            {
                resetPasswordModel.Email = forgotPasswordModel.Email;
                forgotPasswordModel = new ForgotPasswordViewModel();
                StateHasChanged();

                await serverSideForgotPasswordValidator.DisplayMessageAsync(response.Content);
            }
            else
                await serverSideForgotPasswordValidator.DisplayMessagesAsync(response.Content);
        }

        private async Task ResetPassword()
        {
            var response = await PublicClient.PostAsJsonAsync("/api/account/resetPassword", resetPasswordModel);

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
