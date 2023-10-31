using EyEServer.Constants;
using EyEServer.Controllers.Common;
using Memory.Helpers;
using Memory.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace EyEServer.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class MusicController : Database<MusicModel>
{
    [HttpPut("[action]")]
    public async Task<IActionResult> AddIfNotExistAsync(MusicModel model)
    {
        if (await GetItems().FirstOrDefaultAsync(i => i.DiscogsId == model.DiscogsId) == null)
        {
            await DiscogsHelper.SetImageSourceAsync(model, _clientFactory.CreateClient("localClient"));
            return await PostAsync(model);
        }

        return BadRequest(_localizer["ObjectExist"]);
    }

    [HttpPost("[action]")]
    public virtual async Task<IActionResult> UpdateImageSourcesAsync()
    {
        foreach (var item in await GetItems().ToListAsync())
        {
            await DiscogsHelper.SetImageSourceAsync(item, _clientFactory.CreateClient(HttpClientNames.LOCAL_CLIENT));
            _database.Update(item);
            await _database.SaveChangesAsync();
        }

        return NoContent();
    }

    public override DbSet<MusicModel> GetItems()
    {
        return _database.Music;
    }
}
