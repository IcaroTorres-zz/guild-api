using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Maps
{
    internal class InviteMap : IEntityTypeConfiguration<Invite>
    {
        public void Configure(EntityTypeBuilder<Invite> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne("member")
                .WithMany()
                .HasForeignKey("MemberId")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}