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
			builder.HasOne(x => x.Guild)
				.WithMany(x => x.Invites)
				.HasForeignKey(x => x.GuildId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.Member)
				.WithMany()
				.HasForeignKey(x => x.MemberId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.Property(x => x.CreatedDate)
				.ValueGeneratedOnAdd()
				.HasDefaultValueSql("CURRENT_TIMESTAMP");
			builder.Property(x => x.ModifiedDate)
				.ValueGeneratedOnUpdate()
				.HasDefaultValueSql("CURRENT_TIMESTAMP");
		}
	}
}