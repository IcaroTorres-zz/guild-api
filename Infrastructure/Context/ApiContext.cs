using Infrastructure.Maps;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Context
{
    public class ApiContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("dbo")
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
                .SeedEntities()
                .EnableGuidToStringConversion();

            base.OnModelCreating(builder);
        }
    }
}