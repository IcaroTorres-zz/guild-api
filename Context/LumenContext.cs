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
    public LumenContext() {}
    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseInMemoryDatabase("lumenInMemoryDB");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>()
        .HasOne(u => u.Guild)
        .WithOne(g => g.Master)
        .HasForeignKey<User>(u => u.GuildName);

      modelBuilder.Entity<Guild>()
        .HasOne(g => g.Master)
        .WithOne(u => u.Guild)
        .HasForeignKey<Guild>(g => g.MasterName);
    }
  }
}