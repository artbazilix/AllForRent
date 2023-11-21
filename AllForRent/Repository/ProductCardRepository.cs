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
			return await _context.ProductCards
                .Include(p => p.Image)
                .Include(p => p.Address)
                .ToListAsync();
		}

		public async Task<ProductCard?> GetByIdAsync(int id)
		{
			return await _context.ProductCards
                .Include(p => p.Image)
                .Include(p => p.Address)
                .FirstOrDefaultAsync(x => x.Id == id);
		}

        public async Task<ProductCard?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.ProductCards
                .Include(p => p.Image)
                .Include(p => p.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public bool Add(ProductCard productCard)
        {
            _context.Add(productCard);
            return Save();
        }

        public bool Delete(ProductCard productCard)
        {
            if (productCard.Image != null)
            {
                _context.Remove(productCard.Image);
            }

            if (productCard.Address != null)
            {
                _context.Remove(productCard.Address);
            }

            _context.Remove(productCard);
            return Save();
        }

        public bool Update(ProductCard productCard)
        {
            if (productCard.Image != null)
            {
                _context.Entry(productCard.Image).State = EntityState.Modified;
            }

            if (productCard.Address != null)
            {
                _context.Entry(productCard.Address).State = EntityState.Modified;
            }

            _context.Entry(productCard).State = EntityState.Modified;
            return Save();
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Произошла ошибка при сохранении изменений: " + ex.Message);
                return false;
            }
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

		public async Task<ProductCard?> GetByIdWithAddressAndImageAsync(int id)
		{
			return await _context.ProductCards
				.Include(p => p.Address)
				.Include(p => p.Image)
				.FirstOrDefaultAsync(x => x.Id == id);
		}

        public IQueryable<ProductCard> SearchByName(string searchTerm)
        {
            return _context.ProductCards
                           .Where(p => p.HeadTitle.Contains(searchTerm))
                           .AsQueryable();
        }
    }
}
