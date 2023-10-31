using EyEServer.Controllers.Common;
using Memory.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EyEServer.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class TextsController : Database<TextModel>
{
    public override DbSet<TextModel> GetItems()
    {
        return _database.Texts;
    }
}
