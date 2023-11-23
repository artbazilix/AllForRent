using AllForRent.Data;
using AllForRent.Interfaces;
using AllForRent.Models;
using AllForRent.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllForRent.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _usersRepository;
        private readonly IProductCardRepository _productCardRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AppDbContext _context;

        public UserController(IUserRepository userRepository, IProductCardRepository productCardRepository, IPhotoService photoService, IHttpContextAccessor contextAccessor, AppDbContext context)
        {
            _usersRepository = userRepository;
            _productCardRepository = productCardRepository;
            _photoService = photoService;
            _contextAccessor = contextAccessor;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _usersRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    City = user.Address?.City
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _usersRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Street = user.Address?.Street,
                City = user.Address?.City,
                State = user.Address?.State,
                ProfileImageUrl = user.ProfileImageUrl
            };
            return View(userDetailViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var curUserId = _contextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateProductCardViewModel { AppUserId = curUserId };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCardViewModel productCardVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ошибка загрузки изображения");
                return View(productCardVM);
            }

            var productCardImages = await AddImages(productCardVM);

            if (!string.IsNullOrEmpty(productCardVM.AppUserId))
            {
                var productCard = CreateProductCard(productCardVM, productCardImages);
                _productCardRepository.Add(productCard);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("", "Ошибка: некоторые данные отсутствуют");
                return View(productCardVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var productCard = await GetProductCard(id);
            if (productCard == null) return View("Error");

            var productCardVM = CreateViewModel(productCard);

            if (productCard.Address != null)
            {
                productCardVM.Street = productCard.Address.Street;
                productCardVM.City = productCard.Address.City;
                productCardVM.State = productCard.Address.State;
            }

            return View(productCardVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditProductCardViewModel productCardVM)
        {
            Console.WriteLine("Edit method called");  // Добавьте эту строку
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ошибка редактирования");
                return View("Error");
            }

            var sellerCard = await _productCardRepository.GetByIdAsync(id);

            if (sellerCard != null)
            {
                await UpdateImages(sellerCard, productCardVM);

                sellerCard.HeadTitle = productCardVM.HeadTitle;
                sellerCard.Description = productCardVM.Description;
                sellerCard.Price = productCardVM.Price;

                if (sellerCard.Image != null)
                {
                    _context.Entry(sellerCard.Image).State = EntityState.Modified;
                }

                if (sellerCard.Address != null)
                {
                    sellerCard.Address.Street = productCardVM.Street;
                    sellerCard.Address.City = productCardVM.City;
                    sellerCard.Address.State = productCardVM.State;
                    _context.Entry(sellerCard.Address).State = EntityState.Modified;
                }

                _context.Entry(sellerCard).State = EntityState.Modified;
                _productCardRepository.Save();
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return View(productCardVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productCardDetails = await _productCardRepository.GetByIdAsync(id);
            if (productCardDetails == null) return View("Error");
            return View(productCardDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteProductCard(int id)
        {
            var productCardDetails = await _productCardRepository.GetByIdAsyncNoTracking(id);
            if (productCardDetails == null) return View("Error");

            _productCardRepository.Delete(productCardDetails);

            return RedirectToAction("Index", "Dashboard");
        }

        private async Task<ProductCardImages> AddImages(CreateProductCardViewModel productCardVM)
        {
            var productCardImages = new ProductCardImages();
            var imageProperties = new List<string> { "First", "Second", "Third", "Fourth", "Fifth" };
            int index = 0;

            if (productCardVM.Images != null)
            {
                foreach (var image in productCardVM.Images)
                {
                    var result = await _photoService.AddPhotoAsync(image);
                    if (result != null && index < imageProperties.Count)
                    {
                        typeof(ProductCardImages).GetProperty(imageProperties[index]).SetValue(productCardImages, result.Url.ToString());
                        index++;
                    }
                }
            }
            return productCardImages;
        }


        private ProductCard CreateProductCard(CreateProductCardViewModel productCardVM, ProductCardImages productCardImages)
        {
            var address = _context.Addresses.Add(productCardVM.Address);
            _context.SaveChanges();

            return new ProductCard
            {
                HeadTitle = productCardVM.Name,
                Description = productCardVM.Description,
                Price = productCardVM.Price,
                Image = productCardImages,
                AppUserId = productCardVM.AppUserId,
                AddressId = address.Entity.Id
            };
        }

        private async Task<ProductCard> GetProductCard(int id)
        {
            var productCard = await _productCardRepository.GetByIdAsync(id);
            if (productCard == null || productCard.Image == null) return null;

            var address = productCard.Address;
            if (address != null)
            {
                var street = address.Street;
                var city = address.City;
                var state = address.State;
            }

            return productCard;
        }

        private EditProductCardViewModel CreateViewModel(ProductCard productCard)
        {
            return new EditProductCardViewModel
            {
                HeadTitle = productCard.HeadTitle,
                Description = productCard.Description,
                Price = productCard.Price,
                ImageUrls = new List<string>
        {
            productCard.Image.First,
            productCard.Image.Second,
            productCard.Image.Third,
            productCard.Image.Fourth,
            productCard.Image.Fifth
        }
            };
        }

        private async Task UpdateImages(ProductCard sellerCard, EditProductCardViewModel productCardVM)
        {
            if (sellerCard.Image == null)
            {
                sellerCard.Image = new ProductCardImages();
            }

            // Удаление изображений
            var imageProperties = new List<string> { "First", "Second", "Third", "Fourth", "Fifth" };
            for (int i = 0; i < productCardVM.DeleteImages.Count; i++)
            {
                if (productCardVM.DeleteImages[i])
                {
                    typeof(ProductCardImages).GetProperty(imageProperties[i]).SetValue(sellerCard.Image, string.Empty);
                    _context.Entry(sellerCard.Image).Property(imageProperties[i]).IsModified = true;
                }
            }

            // Добавление или замена изображений
            if (productCardVM.NewImages != null)
            {
                for (int i = 0; i < productCardVM.NewImages.Count; i++)
                {
                    var newImage = productCardVM.NewImages[i];
                    if (newImage != null)
                    {
                        var uploadResult = await _photoService.AddPhotoAsync(newImage);
                        if (uploadResult != null && uploadResult.Url != null)
                        {
                            typeof(ProductCardImages).GetProperty(imageProperties[i]).SetValue(sellerCard.Image, uploadResult.Url.ToString());
                            _context.Entry(sellerCard.Image).Property(imageProperties[i]).IsModified = true;
                        }
                    }
                }
            }

            // Сохранение изменений в базе данных
            _context.SaveChanges();
        }
    }
}
