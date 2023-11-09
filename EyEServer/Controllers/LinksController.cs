using EyEServer.Constants;
using EyEServer.Controllers.Common;
using EyEServer.Data;
using MemoryLib.Enums;
using MemoryLib.Helpers;
using MemoryLib.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
namespace EyEServer.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class LinksController(ApplicationDbContext database, IHttpClientFactory _clientFactory)
    : AdminLinksController<LinkModel>(database)
{
    [HttpPut("[action]")]
    public async Task<IActionResult> AddIfNotExistAsync(LinkModel model)
    {
        if (await GetItems().FirstOrDefaultAsync(i => i.Link == model.Link && i.FolderName == model.FolderName) == null)
        {
            var result = true;
            var client = _clientFactory.CreateClient(HttpClientNames.LOCAL_CLIENT);

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

        return BadRequest(RequestsResource.ObjectExist);
    }

    public override DbSet<LinkModel> GetItems()
    {
        return _database.Links;
    }
}
