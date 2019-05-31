using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using api.Models;
using System.Collections.Generic;

namespace api.Context {
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
        .HasForeignKey<Guild>(g => g.MasterName);
        // the foreignKey here is needed cause there is no navigation property on the other relation size
    }
  }
}