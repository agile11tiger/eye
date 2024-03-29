﻿using EyEServer.Constants;
using EyEServer.Controllers.Common;
using EyEServer.Data;
using MemoryLib.Helpers;
using MemoryLib.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
namespace EyEServer.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class MusicController(ApplicationDbContext database, IHttpClientFactory _clientFactory)
    : Database<MusicModel>(database)
{
    [HttpPut("[action]")]
    public async Task<IActionResult> AddIfNotExistAsync(MusicModel model)
    {
        if (await GetItems().FirstOrDefaultAsync(i => i.DiscogsId == model.DiscogsId) == null)
        {
            await DiscogsHelper.SetImageSourceAsync(model, _clientFactory.CreateClient("localClient"));
            return await PostAsync(model);
        }

        return BadRequest(RequestsResource.ObjectExist);
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
