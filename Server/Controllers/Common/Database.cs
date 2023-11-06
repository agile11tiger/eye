using EyEServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Net.Http;
using System.Threading.Tasks;
namespace EyEServer.Controllers.Common;

public abstract class Database<T> : Controller where T : class, IDatabaseItem, new()
{
    protected Database()
    {
        _database = Program.ServiceProvider.GetService<ApplicationDbContext>();
        _clientFactory = Program.ServiceProvider.GetService<IHttpClientFactory>();
    }

    protected readonly ApplicationDbContext _database;
    protected readonly IHttpClientFactory _clientFactory;

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
        await _database.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        GetItems().Remove(new T() { Id = id });
        await _database.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(T item)
    {
        _database.Update(item);
        await _database.SaveChangesAsync();
        return NoContent();
    }

    public abstract DbSet<T> GetItems();
}
