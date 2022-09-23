using EyE.Server.Constants;
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
    public class MusicController : Database<MusicModel>
    {
        public MusicController(
            ApplicationDbContext db,
            IHttpClientFactory clientFactory)
            : base(db, clientFactory)
        {
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> AddIfNotExistAsync(MusicModel model)
        {
            if (await GetItems().FirstOrDefaultAsync(i => i.DiscogsId == model.DiscogsId) == null)
            {
                await DiscogsHelper.SetImageSourceAsync(model, ClientFactory.CreateClient("localClient"));
                return await PostAsync(model);
            }

            return BadRequest("Объект уже существует");
        }

        [HttpPost("[action]")]
        public virtual async Task<IActionResult> UpdateImageSourcesAsync()
        {
            foreach(var item in await GetItems().ToListAsync())
            {
                await DiscogsHelper.SetImageSourceAsync(item, ClientFactory.CreateClient(HttpClientNames.LOCAL_CLIENT));
                Db.Update(item);
                await Db.SaveChangesAsync();
            }

            return NoContent();
        }

        public override DbSet<MusicModel> GetItems()
        {
            return Db.Music;
        }
    }
}
