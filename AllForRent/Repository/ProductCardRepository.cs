using AllForRent.Data;
using AllForRent.Interfaces;
using AllForRent.Models;
using Microsoft.EntityFrameworkCore;

namespace AllForRent.Repository
{
    public class ProductCardRepository : IProductCardRepository
    {
        private readonly AppDbContext _context;
        public ProductCardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductCard>> GetAll()
        {
			return await _context.ProductCards.Include(p => p.Image).ToListAsync();
		}

		public async Task<ProductCard?> GetByIdAsync(int id)
		{
			return await _context.ProductCards.Include(p => p.Image).FirstOrDefaultAsync(x => x.Id == id);
		}

        public async Task<ProductCard?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.ProductCards.Include(p => p.Image).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool Add(ProductCard productCard)
        {
            _context.Add(productCard);
            return Save();
        }

        public bool Delete(ProductCard productCard)
        {
            _context.Remove(productCard);
            return Save();
        }

        public bool Update(ProductCard productCard)
        {
            _context.Update(productCard);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public async Task<IEnumerable<ProductCard>> GetProductCardByCity(string city)
        {
            return await _context.ProductCards
                .Include(c => c.Address)
				.Include(c => c.Image)
				.Where(c => c.Address.City.Contains(city))
                .Distinct()
                .ToListAsync();
        }
    }
}
