using AllForRent.Models;

namespace AllForRent.Interfaces
{
	public interface IDashboardRepository
	{
		Task<List<ProductCard>> GetAllUserProductCards();
		Task<AppUser> GetUserById(string id);
		Task<AppUser> GetByIdNoTracking(string id);
		bool Update(AppUser user);
		bool Save();
        Task<List<Purchase>> GetPurchasesByUserId(string userId);
        Task Add(Purchase purchase);
    }
}

