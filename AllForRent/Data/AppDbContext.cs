using AllForRent.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AllForRent.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<ProductCard> ProductCards { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProductCardImages> ProductCardsImages { get; set; }
    }
}
