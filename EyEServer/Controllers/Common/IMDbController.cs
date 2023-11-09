using EyEServer.Constants;
using EyEServer.Data;
using MemoryLib.Enums;
using MemoryLib.Helpers;
using MemoryLib.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
namespace EyEServer.Controllers.Common;

public abstract class IMDbController<T>(ApplicationDbContext database, IHttpClientFactory _clientFactory)
    : AdminLinksController<T>(database) where T : IMDbModel, new()
{
    [HttpPut("[action]")]
    public async Task<IActionResult> AddIfNotExistAsync(T model)
    {
        //С папкой "CartoonSeriesMyChildhood" допустимы дубликаты
        var items = model.FolderName == FolderNames.CartoonSeriesMyChildhood
            ? GetItems().Where(i => i.FolderName == FolderNames.CartoonSeriesMyChildhood)
            : GetItems().Where(i => i.FolderName != FolderNames.CartoonSeriesMyChildhood);

        if (await items.FirstOrDefaultAsync(i => i.IMDbId == model.IMDbId) == null)
            return await PostAsync(model);

        return BadRequest(RequestsResource.ObjectExist);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateItemsAsync()
    {
        foreach (var currentItem in await GetItems().AsNoTracking().ToListAsync())
        {
            var newItem = await IMDbHelper.GetIMDbModelAsync<T>(currentItem.Link, _clientFactory.CreateClient(HttpClientNames.LOCAL_CLIENT));

            //если по каким-то причинам не удалось получить новые данные
            if (newItem.IMDbId == newItem.Name)
                continue;

            newItem.Id = currentItem.Id;
            newItem.AddingDate = currentItem.AddingDate;
            newItem.FolderName = currentItem.FolderName;
            _database.Update(newItem);
            await _database.SaveChangesAsync();
        }

        return NoContent();
    }
}
