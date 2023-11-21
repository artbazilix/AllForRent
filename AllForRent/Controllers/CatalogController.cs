using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AllForRent.Interfaces;
using AllForRent.Models;
using Microsoft.AspNetCore.Mvc;
using AllForRent.ViewModels;
using AllForRent.Repository;

namespace AllForRent.Controllers
{
	public class CatalogController : Controller
	{
		private readonly IProductCardRepository _productCardRepository;
		private readonly IUserRepository _userRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IDashboardRepository _dashboardRepository;

		public CatalogController(IProductCardRepository productCardRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IDashboardRepository dashboardRepository)
		{
			_productCardRepository = productCardRepository;
			_userRepository = userRepository;
			_httpContextAccessor = httpContextAccessor;
            _dashboardRepository = dashboardRepository;
        }

		public async Task<IActionResult> Index()
		{
			IEnumerable<ProductCard> productCards = await _productCardRepository.GetAll();
			return View(productCards);
		}

		public async Task<IActionResult> Detail(int id)
		{
			var productCard = await _productCardRepository.GetByIdWithAddressAndImageAsync(id);
			return productCard == null ? NotFound() : View(productCard);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Purchase(int id)
		{
			var product = await _productCardRepository.GetByIdWithAddressAndImageAsync(id);
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userRepository.GetUserById(userId);

			var model = new PurchaseViewModel
			{
				UserName = user.FullName,
				UserBalance = user.Balance,
				ProductName = product.HeadTitle,
				ProductPrice = product.Price.HasValue ? product.Price.Value : 0,
				ProductId = product.Id
			};

			return View(model);
		}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ConfirmPurchase(int productId)
        {
            var product = await _productCardRepository.GetByIdWithAddressAndImageAsync(productId);
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.GetUserById(userId);

            if (product.Price.HasValue)
            {
                if (user.Balance < product.Price.Value)
                {
                    var errorModel = new ErrorViewModel { Message = "У вас недостаточно средств для покупки этого товара" };
                    return View("Error", errorModel);
                }

                user.Balance -= product.Price.Value;
            }
            else
            {
                var errorModel = new ErrorViewModel { Message = "Цена товара не указана" };
                return View("Error", errorModel);
            }

            // Создайте новую покупку и сохраните ее в базе данных
            var purchase = new Purchase
            {
                UserId = userId,
                UserName = user.FullName,
                ProductName = product.HeadTitle,
                ProductPrice = product.Price.HasValue ? product.Price.Value : 0,
                PurchaseTime = DateTime.Now
            };
            await _dashboardRepository.Add(purchase);

            _userRepository.Update(user);

            return RedirectToAction("PurchaseHistory", "Dashboard");
        }

    }

}
