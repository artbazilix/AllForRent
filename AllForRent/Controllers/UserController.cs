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
                    City = user.Address.City
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
                City = user.Address?.City
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
                var productCardImages = new ProductCardImages();
                foreach (var image in productCardVM.Images)
                {
                    var result = await _photoService.AddPhotoAsync(image);
                    if (result != null)
                    {
                        if (string.IsNullOrEmpty(productCardImages.First))
                            productCardImages.First = result.Url.ToString();
                        else if (string.IsNullOrEmpty(productCardImages.Second))
                            productCardImages.Second = result.Url.ToString();
                        else if (string.IsNullOrEmpty(productCardImages.Third))
                            productCardImages.Third = result.Url.ToString();
                        else if (string.IsNullOrEmpty(productCardImages.Fourth))
                            productCardImages.Fourth = result.Url.ToString();
                        else if (string.IsNullOrEmpty(productCardImages.Fifth))
                            productCardImages.Fifth = result.Url.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(productCardVM.AppUserId))
                {
                    var productCard = new ProductCard
                    {
                        HeadTitle = productCardVM.Name,
                        Description = productCardVM.Description,
                        Price = productCardVM.Price,
                        Image = productCardImages,
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
        Name = productCard.HeadTitle,
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
                if (productCardVM.Images != null && productCardVM.Images.Count > 0)
                {
                    try
                    {
                        // Удалить старые изображения
                        if (!string.IsNullOrEmpty(sellerCard.Image.First))
                            await _photoService.DeletePhotoAsync(sellerCard.Image.First);
                        if (!string.IsNullOrEmpty(sellerCard.Image.Second))
                            await _photoService.DeletePhotoAsync(sellerCard.Image.Second);
                        if (!string.IsNullOrEmpty(sellerCard.Image.Third))
                            await _photoService.DeletePhotoAsync(sellerCard.Image.Third);
                        if (!string.IsNullOrEmpty(sellerCard.Image.Fourth))
                            await _photoService.DeletePhotoAsync(sellerCard.Image.Fourth);
                        if (!string.IsNullOrEmpty(sellerCard.Image.Fifth))
                            await _photoService.DeletePhotoAsync(sellerCard.Image.Fifth);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Не удалось удалить фотографию");
                        return View("Error");
                    }

                    // Добавить новые изображения
                    foreach (var image in productCardVM.Images)
                    {
                        var photoResult = await _photoService.AddPhotoAsync(image);
                        if (string.IsNullOrEmpty(sellerCard.Image.First))
                            sellerCard.Image.First = photoResult.Url.ToString();
                        else if (string.IsNullOrEmpty(sellerCard.Image.Second))
                            sellerCard.Image.Second = photoResult.Url.ToString();
                        else if (string.IsNullOrEmpty(sellerCard.Image.Third))
                            sellerCard.Image.Third = photoResult.Url.ToString();
                        else if (string.IsNullOrEmpty(sellerCard.Image.Fourth))
                            sellerCard.Image.Fourth = photoResult.Url.ToString();
                        else if (string.IsNullOrEmpty(sellerCard.Image.Fifth))
                            sellerCard.Image.Fifth = photoResult.Url.ToString();
                    }
                }

                sellerCard.HeadTitle = productCardVM.Name;
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

