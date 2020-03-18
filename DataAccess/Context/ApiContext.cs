using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class ApiContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        public ApiContext() { }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Member>().HasKey(m => m.Id);
            builder.Entity<Member>().Property<Guid?>(m => m.GuildId).IsRequired(false);

            builder.Entity<Member>()
                .HasOne<Guild>(m => m.Guild)
                .WithMany(g => g.Members)
                .HasForeignKey(m => m.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Guild>()
                .HasMany(g => g.Members)
                .WithOne(m => m.Guild)
                .HasForeignKey(m => m.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Invite>().HasKey(ms => ms.Id);

            builder.Entity<Invite>()
                .HasOne(ms => ms.Guild)
                .WithMany(g => g.Invites)
                .HasForeignKey(ms => ms.GuildId);

            builder.Entity<Invite>()
                .HasOne(ms => ms.Member)
                .WithMany()
                .HasForeignKey(ms => ms.MemberId);

            builder.Entity<Membership>().HasKey(ms => ms.Id);

            builder.Entity<Membership>()
                .HasOne(ms => ms.Guild)
                .WithMany()
                .HasForeignKey(ms => ms.GuildId);

            builder.Entity<Membership>()
                .HasOne(ms => ms.Member)
                .WithMany(m => m.Memberships)
                .HasForeignKey(ms => ms.MemberId);

            base.OnModelCreating(builder);
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
                    ((BaseEntity)entity.Entity).RegisterCreation();
                }
                else if (entity.State == EntityState.Modified)
                {
                    ((BaseEntity)entity.Entity).RegisterModification();
                }
            }
        }
    }
}