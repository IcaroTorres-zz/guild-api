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
      // explicitly needed to map this one-sided navigation property on Guild Entity
      modelBuilder.Entity<Guild>()
        .HasOne(g => g.Master)
        .WithOne() 
        .HasForeignKey<Guild>(g => g.MasterId);
        // the foreignKey here is needed cause there is no navigation property on the other relation size
    }
  }
}