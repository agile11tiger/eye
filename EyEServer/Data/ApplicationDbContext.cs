using Duende.IdentityServer.EntityFramework.Options;
using EyEServer.Data.Configurations;
using MemoryLib.Models.Common;
using MemoryLib.Models.Review;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace EyEServer.Data;

public class ApplicationDbContext : ApiAuthorizationDbContext<UserModel>
{
    public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
           : base(options, operationalStoreOptions)
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new LinkModelConfiguration());
    }

    public DbSet<AnimeModel> Anime { get; set; }
    public DbSet<MusicModel> Music { get; set; }
    public DbSet<FilmModel> Films { get; set; }
    public DbSet<SerialModel> Serials { get; set; }
    public DbSet<GameModel> Games { get; set; }
    public DbSet<LinkModel> Links { get; set; }
    public DbSet<TextModel> Texts { get; set; }
}
