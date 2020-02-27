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

        public ApiContext() { }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // explicitly needed to map this one-sided navigation property on Guild Entity
            modelBuilder.Entity<Guild>()
              .HasOne(g => g.Master)
              .WithOne()
              .HasForeignKey<Guild>(g => g.MasterId);
            // the foreignKey here is needed cause there is no navigation property on the other relation size
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