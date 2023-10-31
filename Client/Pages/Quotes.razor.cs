using Blazored.LocalStorage;
using Memory.ViewModels;
namespace EyE.Client.Pages;

[Route("/")]
[Route("Quotes")]
public partial class Quotes
{
    [Inject] public PublicHttpClient PublicHttpClient { get; set; }
    [Inject] public ILocalStorageService LocalStorage { get; set; }
    private WikiquoteViewModel WikiquoteViewModel;
    private readonly ItemAdderViewModel ItemAdderViewModel = new();

    protected override async Task OnInitializedAsync()
    {
        WikiquoteViewModel = await LocalStorage.GetItemAsync<WikiquoteViewModel>("quote");

        if (WikiquoteViewModel == default)
            await GetQuotesAsync();

        await base.OnInitializedAsync();
    }

    public async Task GetQuotesAsync()
    {
        var counter = 0;

        while (counter < 3)
        {
            WikiquoteViewModel = await WikiquoteHelper.GetWikiquoteModelAsync(ItemAdderViewModel?.Id, PublicHttpClient);

            if (WikiquoteViewModel != default)
                break;

            counter++;
        }

        await LocalStorage.SetItemAsync("quote", WikiquoteViewModel);
    }
}
