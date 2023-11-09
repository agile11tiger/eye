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
public class SerialsController(ApplicationDbContext database, IHttpClientFactory clientFactory)
    : IMDbController<SerialModel>(database, clientFactory)
{
    public override DbSet<SerialModel> GetItems()
    {
        return _database.Serials;
    }
}
