using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Maps
{
    public class GuildMap : IEntityTypeConfiguration<Guild>
    {
        public void Configure(EntityTypeBuilder<Guild> builder)
        {
            builder.HasMany(g => g.Members).WithOne(m => m.Guild).HasForeignKey(m => m.GuildId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(g => g.CreatedDate).ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            builder.Property(g => g.ModifiedDate).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETUTCDATE()");
        }
    }
}