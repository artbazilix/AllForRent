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
        Task<IEnumerable<ProductCard>> GetProductCardByCity(string city);
		Task<ProductCard?> GetByIdWithAddressAndImageAsync(int id);
        IQueryable<ProductCard> SearchByName(string name);
        Task<string> GetOwnerId(int productId);
    }
}
