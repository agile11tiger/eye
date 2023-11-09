using EyEServer.Controllers.Common;
using EyEServer.Data;
using MemoryLib.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
namespace EyEServer.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class GamesController(ApplicationDbContext database, IHttpClientFactory clientFactory)
    : IMDbController<GameModel>(database, clientFactory)
{
    public override DbSet<GameModel> GetItems()
    {
        return _database.Games;
    }
}
