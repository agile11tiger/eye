using EyE.Server.Controllers.Common;
using EyE.Server.Data;
using EyE.Shared.Enums;
using EyE.Shared.Helpers;
using EyE.Shared.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        private readonly FolderNames[] adminFolders = new[] 
        {
            FolderNames.Schedule,
            FolderNames.TextNotes,
            FolderNames.YoutubeNotes,
            FolderNames.LinkNotes,
        };

        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Client)]
        [AllowAnonymous]
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole(Roles.Admin.ToString()))
            {
                var list = await GetItems().ToListAsync();
                list.Reverse();
                return Ok(list);
            }
            else
            {
                var list = await GetItems()
                    .Where(i => !adminFolders.Contains(i.FolderName))
                    .ToListAsync();
                list.Reverse();
                return Ok(list);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> AddIfNotExistAsync(LinkModel model)
        {
            if (await GetItems().FirstOrDefaultAsync(i => i.Link == model.Link && i.FolderName == model.FolderName) == null)
            {
                var result = true;
                var client = ClientFactory.CreateClient("localClient");

                switch (model.FolderName)
                {
                    case FolderNames.AnimeSites:
                    case FolderNames.MusicSites:
                    case FolderNames.FilmSites:
                    case FolderNames.SerialSites:
                        result = await LinkHelper.TrySetTitleAndImageAsync(model, client); break;
                    case FolderNames.AnimeClips:
                    case FolderNames.BieutifulSongs:
                    case FolderNames.BieutifulVideos:
                    case FolderNames.NightcoreMusic:
                    case FolderNames.UnusualMusic:
                    case FolderNames.RelaxingMusic:
                    case FolderNames.ConcentratingMusic:
                    case FolderNames.Threads:
                    case FolderNames.Schedule:
                    case FolderNames.TextNotes:
                    case FolderNames.YoutubeNotes:
                        break;
                    default:
                        result = await LinkHelper.TrySetNameAndFaviconAsync(model, client); break;
                }

                if (result == false)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Что-то пошло не так");

                return await PostAsync(model);
            }

            return BadRequest("Объект уже существует");
        }

        public override DbSet<LinkModel> GetItems()
        {
            return Db.Links;
        }
    }
}
