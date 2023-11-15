using AllForRent.Models;
using AllForRent.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AllForRent.ViewModels;
using Microsoft.AspNetCore.Identity;

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
                    Price = productCardVM.Price,
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

        public async Task<IActionResult> Edit(int id) 
        {
            var productCard = await _productCardRepository.GetByIdAsync(id);
            if (productCard == null) return View("Error");
            var productCardVM = new EditProductCardViewModel
            {
                Name = productCard.Name,
                Description = productCard.Description,
                Price = productCard.Price,
                URL = productCard.Image
            };
            return View(productCardVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditProductCardViewModel productCardVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ошибка редактирования");
                return View("Error");
            }

            var sellerCard = await _productCardRepository.GetByIdAsyncNoTracking(id);

            if (sellerCard != null)
            {
                if (productCardVM.Image != null)
                {
                    try
                    {
                        await _photoService.DeletePhotoAsync(sellerCard.Image);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Не удалось удалить фотографию");
                        return View("Error");
                    }

                    var photoResult = await _photoService.AddPhotoAsync(productCardVM.Image);
                    sellerCard.Image = photoResult.Url.ToString();
                }

                sellerCard.Name = productCardVM.Name;
                sellerCard.Description = productCardVM.Description;
                sellerCard.Price = productCardVM.Price;

                _productCardRepository.Update(sellerCard);
                return RedirectToAction("Offers");
            }
            else
            {
                return View(productCardVM);
            }
        }

    }
}
