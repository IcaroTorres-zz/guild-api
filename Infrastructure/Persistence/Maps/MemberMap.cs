using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Maps
{
    internal class MemberMap : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(u => u.Name)
                .IsUnique();
            builder.Property(x => x.Name)
                .IsRequired(true);
            builder.Property(x => x.GuildId)
                .IsRequired(false);

            builder.HasMany(x => x.Memberships)
                .WithOne()
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}