using EyEServer.Controllers.Common;
using EyEServer.Data;
using Memory.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
namespace EyEServer.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class FilmsController(ApplicationDbContext database, IHttpClientFactory clientFactory)
    : IMDbController<FilmModel>(database, clientFactory)
{
    public override DbSet<FilmModel> GetItems()
    {
        return _database.Films;
    }
}
