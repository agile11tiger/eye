using EyEServer.Constants;
using EyEServer.Controllers.Common;
using EyEServer.Data;
using MemoryLib.Helpers;
using MemoryLib.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
namespace EyEServer.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class AnimeController(ApplicationDbContext database, IHttpClientFactory _clientFactory)
    : Database<AnimeModel>(database)
{
    [HttpPut("[action]")]
    public async Task<IActionResult> AddIfNotExistAsync(AnimeModel model)
    {
        if (await GetItems().FirstOrDefaultAsync(i => i.AniDbId == model.AniDbId) == null)
        {
            var result = await AniDbHelper.TrySetValuesAsync(model, _clientFactory.CreateClient(HttpClientNames.LOCAL_CLIENT));

            if (result == false)
                return StatusCode(StatusCodes.Status500InternalServerError, "Что-то пошло не так");

            return await PostAsync(model);
        }

        return BadRequest(RequestsResource.ObjectExist);
    }

    public override DbSet<AnimeModel> GetItems()
    {
        return _database.Anime;
    }
}
