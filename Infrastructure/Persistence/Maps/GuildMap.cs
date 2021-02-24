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
			builder.HasMany(x => x.Members)
				.WithOne(x => x.Guild)
				.HasForeignKey(x => x.GuildId)
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