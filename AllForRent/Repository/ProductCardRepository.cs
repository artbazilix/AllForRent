using AllForRent.Data;
using AllForRent.Interfaces;
using AllForRent.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            return await _context.ProductCards.ToListAsync();
        }
		public async Task<ProductCard?> GetByIdAsync(int id)
		{
			return await _context.ProductCards.FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
