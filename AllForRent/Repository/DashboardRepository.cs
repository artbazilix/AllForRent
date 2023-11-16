using Microsoft.EntityFrameworkCore;
using AllForRent.Data;
using AllForRent.Interfaces;
using AllForRent.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AllForRent.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ProductCard>> GetAllUserProductCards()
		{
			var curUser = _httpContextAccessor.HttpContext?.User;
            var userProductCards = await _context.ProductCards.Include(p => p.Image).Where(p => p.AppUser.Id == curUser.GetUserId()).ToListAsync();
            return userProductCards.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}