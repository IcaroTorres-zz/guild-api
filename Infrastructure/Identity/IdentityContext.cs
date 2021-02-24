using Application.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
