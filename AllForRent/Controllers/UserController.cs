using AllForRent.Data;
using AllForRent.Interfaces;
using AllForRent.Models;
using AllForRent.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AllForRent.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _usersRepository;
        private readonly IProductCardRepository _productCardRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserController(IUserRepository userRepository, IProductCardRepository productCardRepository, IPhotoService photoService, IHttpContextAccessor contextAccessor)
        {
            _usersRepository = userRepository;
            _productCardRepository = productCardRepository;
            _photoService = photoService;
            _contextAccessor = contextAccessor;
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
                    City = user.City
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
                // Обработка ошибки: пользователь не найден
                return NotFound();
            }
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                City = user.City
            };
            return View(userDetailViewModel);
        }


        public IActionResult Create()
        {
            var curUserId = _contextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateProductCardViewModel { AppUserId = curUserId };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCardViewModel productCardVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(productCardVM.Image);

                if (result != null && !string.IsNullOrEmpty(productCardVM.AppUserId))
                {
                    var productCard = new ProductCard
                    {
                        Name = productCardVM.Name,
                        Description = productCardVM.Description,
                        Price = productCardVM.Price,
                        Image = result.Url.ToString(),
                        AppUserId = productCardVM.AppUserId,
                    };
                    _productCardRepository.Add(productCard);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка: некоторые данные отсутствуют");
                }
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
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return View(productCardVM);
            }
        }
    }
}
