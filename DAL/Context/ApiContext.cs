using System.Reflection;
using DAL.Maps;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
	public class ApiContext : DbContext
	{
		public ApiContext(DbContextOptions<ApiContext> options) : base(options)
		{
		}

		public DbSet<Guild> Guilds { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<Invite> Invites { get; set; }
		public DbSet<Membership> Memberships { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder
				.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
				.SeedEntities();

			base.OnModelCreating(builder);
		}
	}
}