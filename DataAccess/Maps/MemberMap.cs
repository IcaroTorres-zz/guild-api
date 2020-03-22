using System;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Maps
{
    public class MemberMap : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property<Guid?>(m => m.GuildId).IsRequired(false);
            builder.Property(m => m.CreatedDate).ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            builder.Property(m => m.ModifiedDate).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETUTCDATE()");
            builder.HasOne<Guild>(m => m.Guild).WithMany(g => g.Members).HasForeignKey(m => m.GuildId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}