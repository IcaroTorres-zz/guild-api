using Domain.Entities;
using DAL.Maps;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DAL.Context
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
            builder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
                .SeedEntities();

            base.OnModelCreating(builder);
        }
    }
}