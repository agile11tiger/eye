namespace EyE.Client.Pages;

[Route("Games/{StrFolderName}")]
public partial class Games
{
    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync("api/Games");
    }
}
