using EyE.Client.Enums;
using EyE.Shared.Extensions;
using EyE.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EyE.Client.Pages
{
    [Route("Films/{StrFolderName}")]
    public partial class Films
    {
        protected override async Task OnInitializedAsync()
        {
            await InitializeAsync("api/Films");
        }
    }
}
