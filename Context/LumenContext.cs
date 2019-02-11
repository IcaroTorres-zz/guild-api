using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using lumen.api.Models;
using System.Collections.Generic;

namespace lumen.api.Context {
  public class LumenContext : DbContext
  {
    public DbSet<Guild> Guilds { get; set; }
    public DbSet<User> Users { get; set; }
    public LumenContext(DbContextOptions<LumenContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Guild>()
        .HasOne(g => g.Master)
        .WithOne() // without any navigation property on User entity
        .HasForeignKey<Guild>(g => g.MasterId);
        // the foreignKey here is needed cause there is no navigation property on the other relation size

      modelBuilder.Entity<Guild>()
        .HasMany(g => g.Members) // navigation property 1:*
        .WithOne(u => u.Guild); // with Guild reference navigation property on User entity
    }
  }
}