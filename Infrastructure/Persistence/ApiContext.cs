using Domain.Models;
using Domain.Nulls;
using Infrastructure.Persistence.Maps;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence
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
                .Ignore<NullMembership>();

            base.OnModelCreating(builder);
        }
    }
}