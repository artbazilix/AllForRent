using AllForRent.Models;
using System.Diagnostics;

namespace AllForRent.Interfaces
{
    public interface IProductCardRepository
    {
        Task<IEnumerable<ProductCard>> GetAll();
		Task<ProductCard?> GetByIdAsync(int id);

	}
}
