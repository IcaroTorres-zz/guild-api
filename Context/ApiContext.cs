using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Context
{
    public class ApiContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        public ApiContext() { }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Membership>().HasKey(bc => new { bc.MemberId, bc.GuildId });

            modelBuilder.Entity<Membership>()
                .HasOne(ms => ms.Guild)
                .WithMany(g => g.Memberships)
                .HasForeignKey(ms => ms.GuildId);

            modelBuilder.Entity<Membership>()
                .HasOne(ms => ms.Member)
                .WithOne(m => m.Membership)
                .HasForeignKey<Membership>(ms => ms.MemberId);
            // the foreignkey is in the membership due to its dependency on User table

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            AddAudit();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAudit();
            return await base.SaveChangesAsync();
        }

        private void AddAudit()
        {
            var entities = ChangeTracker.Entries()
                                        .Where(x => x.Entity is BaseEntity
                                               && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedDate = DateTime.UtcNow;
                } ((BaseEntity)entity.Entity).ModifiedDate = DateTime.UtcNow;
            }
        }
    }
}