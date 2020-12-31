using EyE.Server.Controllers.Common;
using EyE.Server.Data;
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
    public class FilmsController : Database<FilmModel>
    {
        public FilmsController(
            ApplicationDbContext db,
            IHttpClientFactory clientFactory)
            : base(db, clientFactory)
        {
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> PutAsync(FilmModel model)
        {
            if (await GetItems().FirstOrDefaultAsync(i => i.IMDbId == model.IMDbId) == null)
                return await AddAsync(model);

            return BadRequest("Объект уже существует");
        }

        public override DbSet<FilmModel> GetItems()
        {
            return Db.Films;
        }
    }
}
