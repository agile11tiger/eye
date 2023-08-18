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
        public UserChecker(IJSRuntime js, ServerAuthenticationStateProvider authenticationStateProvider)
        {
            JS = js;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        private readonly ServerAuthenticationStateProvider authenticationStateProvider;
        public IJSRuntime JS { get; set; }

        public async Task<bool> CheckNullOrWhiteSpaceAsync(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
                return true;

            await ShowErrorAlertNotAllowNullOrWhiteSpaceAsync();
            return false;
        }

        public async Task<bool> CheckAdminRoleAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();

            if (authState.User.IsInRole(Roles.Admin.ToString()))
                return true;

            await ShowErrorAlertAllowOnlyAdminAsync();
            return false;
        }

        public async Task ShowErrorAdminPageAsync()
        {
            await JS.InvokeVoidAsync("alert", $"Данные текущей страницы доступны только администратору!");
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
