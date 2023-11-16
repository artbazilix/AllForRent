using AllForRent.Models;
using System.Diagnostics;

namespace AllForRent.Interfaces
{
    public interface IProductCardRepository
    {
        Task<IEnumerable<ProductCard>> GetAll();
		Task<ProductCard?> GetByIdAsync(int id);
        Task<ProductCard?> GetByIdAsyncNoTracking(int id);
        bool Add(ProductCard productCard);
        bool Update(ProductCard productCard);
        bool Delete(ProductCard productCard);
        bool Save();
    }
}
