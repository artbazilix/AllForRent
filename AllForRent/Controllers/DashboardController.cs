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

        public DashboardController(IDashboardRepository dashboardRespository, IPhotoService photoService)
        {
            _dashboardRespository = dashboardRespository;
            _photoService = photoService;
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
    }
}
