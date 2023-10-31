using EyEServer.Controllers.Common;
using Memory.Models.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EyEServer.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class SerialsController : IMDbController<SerialModel>
{
    public override DbSet<SerialModel> GetItems()
    {
        return _database.Serials;
    }
}
