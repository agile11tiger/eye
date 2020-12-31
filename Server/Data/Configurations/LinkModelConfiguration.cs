using EyE.Shared.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EyE.Server.Data.Configurations
{
    public class LinkModelConfiguration : IEntityTypeConfiguration<LinkModel>
    {
        public void Configure(EntityTypeBuilder<LinkModel> builder)
        {
        }
    }
}
