using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Maps
{
    internal class GuildMap : IEntityTypeConfiguration<Guild>
    {
        public void Configure(EntityTypeBuilder<Guild> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(u => u.Name)
                .IsUnique();
            builder.Property(x => x.Name)
                .IsRequired(true);

            builder.HasMany(x => x.Members)
                .WithOne("guild")
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Invites)
                .WithOne("guild")
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}