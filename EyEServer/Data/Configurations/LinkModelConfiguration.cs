using MemoryLib.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EyEServer.Data.Configurations;

public class LinkModelConfiguration : IEntityTypeConfiguration<LinkModel>
{
    public void Configure(EntityTypeBuilder<LinkModel> builder)
    {
    }
}
