using AllForRent.Models;
using AllForRent.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AllForRent.ViewModels;

namespace AllForRent.Controllers
{
    public class AccountController : Controller
    {
        private readonly IProductCardRepository _productCardRepository;
        private readonly IPhotoService _photoService;
        public AccountController(IProductCardRepository productCardRepository, IPhotoService photoService)
        {
            _productCardRepository = productCardRepository;
            _photoService = photoService;
        }
        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCardViewModel productCardVM)
        {
            if (ModelState.IsValid) 
            {
                var result = await _photoService.AddPhotoAsync(productCardVM.Image);

                var productCard = new ProductCard
                {
                    Name = productCardVM.Name,
                    Description = productCardVM.Description,
                    Seller = productCardVM.Seller,
                    ProductPrice = productCardVM.ProductPrice,
                    Image = result.Url.ToString()
                };
                _productCardRepository.Add(productCard);
                return RedirectToAction("Offers");
            }
            else 
            {
                ModelState.AddModelError("", "Ошибка загрузки изображения");
            }
            return View(productCardVM);
        }
    }
}
