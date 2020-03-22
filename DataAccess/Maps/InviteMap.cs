using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Maps
{
    public class InviteMap : IEntityTypeConfiguration<Invite>
    {
        public void Configure(EntityTypeBuilder<Invite> builder)
        {
            builder.HasKey(i => i.Id);
            builder.HasOne(i => i.Guild).WithMany(g => g.Invites).HasForeignKey(i => i.GuildId);
            builder.HasOne(i => i.Member).WithMany().HasForeignKey(i => i.MemberId);
            builder.Property(i => i.CreatedDate).ValueGeneratedOnAdd().HasDefaultValueSql("GETUTCDATE()");
            builder.Property(i => i.ModifiedDate).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("GETUTCDATE()");
        }
    }
}