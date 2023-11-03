namespace MemoryClient.Pages;

[Route("Films/{StrFolderName}")]
public partial class Films
{
    protected override async Task OnInitializedAsync()
    {
        await InitializeAsync("api/Films");
    }
}
