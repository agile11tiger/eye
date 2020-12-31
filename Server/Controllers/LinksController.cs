using EyE.Server.Controllers.Common;
using EyE.Server.Data;
using EyE.Shared.Enums;
using EyE.Shared.Helpers;
using EyE.Shared.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyE.Server.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class LinksController : Database<LinkModel>
    {
        public LinksController(
            ApplicationDbContext db,
            IHttpClientFactory clientFactory)
            : base(db, clientFactory)
        {
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> PutAsync(LinkModel model)
        {
            if (await GetItems().FirstOrDefaultAsync(i => i.Link == model.Link && i.FolderName == model.FolderName) == null)
            {
                switch (model.FolderName)
                {
                    case FolderNames.AnimeSites:
                    case FolderNames.MusicSites:
                    case FolderNames.FilmSites:
                    case FolderNames.SerialSites:
                        await LinkHelper.SetTitleAndImageAsync(model); break;
                    case FolderNames.BieutifulVideos:
                        break;
                    default:
                        await LinkHelper.SetNameAndFaviconAsync(model); break;
                }
                return await AddAsync(model);
            }

            return BadRequest("Объект уже существует");
        }

        public override DbSet<LinkModel> GetItems()
        {
            return Db.Links;
        }
    }
}
