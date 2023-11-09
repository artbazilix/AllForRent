using AllForRent.Models;
using AllForRent.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AllForRent.Controllers
{
    public class AccountController : Controller
    {
        private readonly IProductCardRepository _productCardRepository;
        public AccountController(IProductCardRepository productCardRepository)
        {
            _productCardRepository = productCardRepository;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> Offers()
        {
            IEnumerable<ProductCard> productCards = await _productCardRepository.GetAll();
            return View(productCards);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
