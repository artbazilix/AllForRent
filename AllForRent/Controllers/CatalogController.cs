using AllForRent.Interfaces;
using AllForRent.Models;
using Microsoft.AspNetCore.Mvc;

namespace AllForRent.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductCardRepository _productCardRepository;
        public CatalogController(IProductCardRepository productCardRepository)
        {
            _productCardRepository = productCardRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<ProductCard> productCards = await _productCardRepository.GetAll();
            return View(productCards);
        }

		public async Task<IActionResult> Detail(int id)
		{
			var productCard = await _productCardRepository.GetByIdAsync(id);
			return productCard == null ? NotFound() : View(productCard);
		}
    }
}
