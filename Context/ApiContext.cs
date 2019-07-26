using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using api.Models;
using System.Threading.Tasks;
using System.Threading;

namespace api.Context
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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        AddAudit();
        return await base.SaveChangesAsync();
    }

    private void AddAudit()//IHttpContextAccessor httpContextAccessor)
    {
      //var httpContext = httpContextAccessor.HttpContext;
      var entities = ChangeTracker.Entries().Where(x => x.Entity is Entity<int> && (x.State == EntityState.Added || x.State == EntityState.Modified));
      //var currentUsername = (httpContext != null && httpContext.User != null) ? httpContext.User.Identity.Name : "Anonymous";
      foreach (var entity in entities)
      {
        if (entity.State == EntityState.Added)
        {
          ((Entity<int>)entity.Entity).CreatedDate = DateTime.UtcNow;
          //((Entity<int>)entity.Entity).CreatedBy = currentUsername;
        }

        ((Entity<int>)entity.Entity).ModifiedDate = DateTime.UtcNow;
        //((Entity<int>)entity.Entity).ModifiedBy = currentUsername;
      }
    }
  }
}