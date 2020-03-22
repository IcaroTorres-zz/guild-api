using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Maps
{
    public class MembershipMap : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(ms => ms.Id);
            builder.HasOne(ms => ms.Guild).WithMany().HasForeignKey(ms => ms.GuildId);
            builder.HasOne(ms => ms.Member).WithMany(m => m.Memberships).HasForeignKey(ms => ms.MemberId);
            builder.Property(ms => ms.Entrance).ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            builder.Property(ms => ms.Exit).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETUTCDATE()");
        }
    }
}