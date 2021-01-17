using Domain.Models;
using Domain.Models.Nulls;
using Microsoft.EntityFrameworkCore;
using Persistence.Maps;
using System.Reflection;

namespace Persistence.Context
{
    public class ApiContext : DbContext
	{
		public ApiContext(DbContextOptions<ApiContext> options) : base(options)
		{
			ChangeTracker.LazyLoadingEnabled = false;
		}

		public DbSet<Guild> Guilds { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<Invite> Invites { get; set; }
		public DbSet<Membership> Memberships { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder
				.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
				.SeedEntities()
				.Ignore<NullGuild>()
				.Ignore<NullMember>()
				.Ignore<NullInvite>()
				.Ignore<NullMembership>()
				.Ignore<Entities.Guild>()
				.Ignore<Entities.Member>()
				.Ignore<Entities.Invite>()
				.Ignore<Entities.Membership>();

			base.OnModelCreating(builder);
		}
	}
}