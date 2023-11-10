using EyEClientLib.Resources.Localization.Identity;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EyEClientLib.Components;

/// <summary>
/// https://stackoverflow.com/questions/69670507/how-to-check-client-connection-status-in-blazor-web-assembly
/// </summary>
public class ConnectionChecker(IJSRuntime _js, ISnackbar _snackbar) : IAsyncDisposable
{
    public bool IsOnline { get; set; }

    [JSInvokable("ConnectionChecker.StatusChanged")]
    public void OnConnectionStatusChanged(bool isOnline)
    {
        IsOnline = isOnline;
        //StateHasChanged();
    }

    public bool Check()
    {
        if (IsOnline)
            return true;

        _snackbar.Add(IdentityResource.CheckInternetConnection, Severity.Error);
        return false;
    }

    public async Task InitializeAsync()
    {
        await _js.InvokeVoidAsync("ConnectionChecker.Initialize", DotNetObjectReference.Create(this));
    }

    public async ValueTask DisposeAsync()
    {
        await _js.InvokeVoidAsync("Connection.Dispose");
    }

}
