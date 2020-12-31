using EyE.Server.Controllers.Common;
using EyE.Server.Data;
using EyE.Shared.Helpers;
using EyE.Shared.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyE.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class MusicController : Database<MusicModel>
    {
        public MusicController(
            ApplicationDbContext db,
            IHttpClientFactory clientFactory)
            : base(db, clientFactory)
        {
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> PutAsync(MusicModel model)
        {
            if (await GetItems().FirstOrDefaultAsync(i => i.DiscogsId == model.DiscogsId) == null)
            {
                await DiscogsHelper.SetDiscogsImageAsync(model);
                return await AddAsync(model);
            }

            return BadRequest("Объект уже существует");
        }

        public override DbSet<MusicModel> GetItems()
        {
            return Db.Music;
        }
    }
}
