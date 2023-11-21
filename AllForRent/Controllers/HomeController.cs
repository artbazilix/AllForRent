using AllForRent.Helpers;
using AllForRent.Interfaces;
using AllForRent.Models;
using AllForRent.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace AllForRent.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductCardRepository _productCardRepository;

        public HomeController(ILogger<HomeController> logger, IProductCardRepository productCardRepository)
        {
            _logger = logger;
            _productCardRepository = productCardRepository;
        }

		public async Task<IActionResult> Index()
		{
			var ipInfo = new IPInfo();
			var homeViewModel = new HomeViewModel();
			try
			{
				string url = "https://ipinfo.io?token=f1b01083e080f1";
				var info = new WebClient().DownloadString(url);
				ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
				RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
				ipInfo.Country = myRI1.EnglishName;
				homeViewModel.City = ipInfo.City;
				homeViewModel.State = ipInfo.Region;
				if (homeViewModel.City != null)
				{
					homeViewModel.ProductCards = await _productCardRepository.GetProductCardByCity(homeViewModel.City);
				}
				else
				{
					homeViewModel.ProductCards = null;
				}
				return View(homeViewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while loading the product cards.");
				homeViewModel.ProductCards = null;
			}
			return View(homeViewModel);
		}

        public IActionResult Search(string searchTerm)
        {
            var results = _productCardRepository.SearchByName(searchTerm)
                                                .Include(p => p.Image)
                                                .Include(p => p.Address)
                                                .Include(p => p.AppUser);
            return View(results);
        }


        public IActionResult AutoCompleteSearch(string searchTerm)
		{
			var products = _productCardRepository.SearchByName(searchTerm);
			var titles = products.Select(p => p.HeadTitle).ToList();
			Console.WriteLine(string.Join(", ", titles));
			return Json(titles);
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}