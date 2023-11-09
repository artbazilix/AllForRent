using AllForRent.Models;
using Microsoft.EntityFrameworkCore;

namespace AllForRent.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<AppSeller> AppSellers { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ProductCard> ProductCards { get; set; }
    }
}
