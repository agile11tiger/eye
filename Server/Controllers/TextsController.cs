using EyEServer.Controllers.Common;
using EyEServer.Data;
using Memory.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EyEServer.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class TextsController(ApplicationDbContext database) : Database<TextModel>(database)
{
    public override DbSet<TextModel> GetItems()
    {
        return _database.Texts;
    }
}
