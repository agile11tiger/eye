using EyE.Server.Data;
using EyE.Shared.Enums;
using EyE.Shared.Helpers;
using EyE.Shared.Models.Common;
using EyE.Shared.Models.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyE.Server.Controllers.Common
{
    public abstract class AdminLinksController<T> : Database<T> where T : LinkModel, new()
    {
        protected AdminLinksController(
            ApplicationDbContext db, 
            IHttpClientFactory clientFactory)
            :base(db, clientFactory)
        {
        }

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
}
