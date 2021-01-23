using EyE.Shared.Enums;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EyE.Client.Services
{
    public class UserChecker
    {
        public UserChecker(IJSRuntime js, AuthenticationStateProvider authenticationStateProvider)
        {
            JS = js;
            this.authStateProvider = authenticationStateProvider;
        }

        private readonly AuthenticationStateProvider authStateProvider;
        public IJSRuntime JS { get; set; }
        public async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return await authStateProvider.GetAuthenticationStateAsync();
        }

        public async Task<bool> CheckNullOrWhiteSpaceAsync(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
                return true;
            else
            {
                await ShowErrorAlertNotAllowNullOrWhiteSpaceAsync();
                return false;
            }
        }

        public async Task<bool> CheckAdminRoleAsync()
        {
            var authState = await GetAuthenticationStateAsync();

            if (authState.User.IsInRole(Roles.Admin.ToString()))
                return true;
            else
            {
                await ShowErrorAlertAllowOnlyAdminAsync();
                return false;
            }
        }

        public async Task ShowSomethingHappenedAsync()
        {
            await JS.InvokeVoidAsync("alert", $"Что-то пошло не так. Сообщение об ошибке отправлено на сервер");
        }

        public async Task ShowErrorAlertNotAllowNullOrWhiteSpaceAsync()
        {
            await JS.InvokeVoidAsync("alert", $"Поле должно быть заполнено");
        }

        public async Task ShowErrorAlertAllowOnlyAdminAsync()
        {
            await JS.InvokeVoidAsync("alert", $"Это действие разрешено только администратору");
        }

        public async Task ShowErrorAlertAsync(HttpStatusCode statusCode, string text)
        {
            await JS.InvokeVoidAsync("alert", $"Упссс, ошибка {statusCode}! {text}");
        }
    }
}
