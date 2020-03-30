using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Maps
{
    public class MemberMap : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(u => u.Name)
                .IsUnique();
            builder.Property(x => x.GuildId)
                .IsRequired(false);
            builder.Property(x => x.CreatedDate)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.ModifiedDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasOne(x => x.Guild)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}