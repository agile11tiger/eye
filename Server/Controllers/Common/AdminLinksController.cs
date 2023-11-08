using EyEServer.Data;
using Memory.Helpers;
using Memory.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace EyEServer.Controllers.Common;

public abstract class AdminLinksController<T>(ApplicationDbContext database)
    : Database<T>(database) where T : LinkModel, new()
{
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Client)]
    [AllowAnonymous]
    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        List<T> list = User.Identity.IsAuthenticated && User.IsInRole(Roles.Admin.ToString())
            ? list = await GetItems().ToListAsync()
            : list = await GetItems()
                .Where(i => !AdminHelper.AdminFolders.Contains(i.FolderName))
                .ToListAsync();

        list.Reverse();
        return Ok(list);
    }
}
