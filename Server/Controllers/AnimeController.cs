using EyE.Server.Controllers.Common;
using EyE.Server.Data;
using EyE.Shared.Helpers;
using EyE.Shared.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyE.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AnimeController : Database<AnimeModel>
    {
        public AnimeController(
            ApplicationDbContext db,
            IHttpClientFactory clientFactory)
            : base(db, clientFactory)
        {
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> PutAsync(AnimeModel model)
        {
            if (await GetItems().FirstOrDefaultAsync(i => i.AniDbId == model.AniDbId) == null)
            {
                var result = await AniDbHelper.TrySetValuesAsync(model, ClientFactory.CreateClient("localClient"));

                if (result == false) 
                    return StatusCode(StatusCodes.Status500InternalServerError, "Что-то пошло не так");

                return await AddAsync(model);
            }

            return BadRequest("Объект уже существует");
        }

        public override DbSet<AnimeModel> GetItems()
        {
            return Db.Anime;
        }
    }
}
