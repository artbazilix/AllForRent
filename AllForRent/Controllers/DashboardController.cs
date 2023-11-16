using AllForRent.Interfaces;
using AllForRent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllForRent.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRespository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IDashboardRepository dashboardRespository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _dashboardRespository = dashboardRespository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var userProductCards = await _dashboardRespository.GetAllUserProductCards();
            var dashboardViewModel = new DashboardViewModel()
            {
                ProductCards = userProductCards
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRespository.GetUserById(curUserId);
            if (user == null) return View("Error");
            var editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                FullName = user.FullName,
                ProfileImage = user.ProfileImageUrl,
                PhoneNumber = user.PhoneNumber,
                City = user.Address?.City,
                State = user.Address?.State,
                Street = user.Address?.Street
            };
            return View(editUserViewModel);
        }

    }
}
