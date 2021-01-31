using EyE.Server.Data;
using EyE.Shared.Models.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;

namespace EyE.Server.Controllers.Common
{
    public abstract class Database<T> : Controller where T : class, IDatabaseItem, new()
    {
        protected Database(
            ApplicationDbContext db,
            IHttpClientFactory clientFactory)
        {
            Db = db;
            ClientFactory = clientFactory;
        }

        protected readonly ApplicationDbContext Db;
        protected readonly IHttpClientFactory ClientFactory;

        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Client)]
        [AllowAnonymous]
        [HttpGet]
        public virtual async Task<IActionResult> GetAsync()
        {
            var list = await GetItems().ToListAsync();
            list.Reverse();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(T item)
        {
            await GetItems().AddAsync(item);
            await Db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            GetItems().Remove(new T() { Id = id });
            await Db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(T item)
        {
            Db.Update(item);
            await Db.SaveChangesAsync();
            return NoContent();
        }

        public abstract DbSet<T> GetItems();
    }
}
