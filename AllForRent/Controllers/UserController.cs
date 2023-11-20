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
				await UpdateImages(sellerCard, productCardVM);

				sellerCard.HeadTitle = productCardVM.HeadTitle;
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

		private async Task<ProductCardImages> AddImages(CreateProductCardViewModel productCardVM)
		{
			var productCardImages = new ProductCardImages();
			var imageProperties = new List<string> { "First", "Second", "Third", "Fourth", "Fifth" };
			int index = 0;

			foreach (var image in productCardVM.Images)
			{
				var result = await _photoService.AddPhotoAsync(image);
				if (result != null && index < imageProperties.Count)
				{
					typeof(ProductCardImages).GetProperty(imageProperties[index]).SetValue(productCardImages, result.Url.ToString());
					index++;
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

			if (productCardVM.Images != null && productCardVM.Images.Count > 0)
			{
				await DeleteExistingImages(sellerCard);
				await AddNewImages(sellerCard, productCardVM);
			}
		}

		private async Task DeleteExistingImages(ProductCard sellerCard)
		{
			try
			{
				var images = new List<string> { sellerCard.Image.First, sellerCard.Image.Second, sellerCard.Image.Third, sellerCard.Image.Fourth, sellerCard.Image.Fifth };
				foreach (var image in images)
				{
					if (!string.IsNullOrEmpty(image))
						await _photoService.DeletePhotoAsync(image);
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Не удалось удалить фотографию");
				throw;
			}
		}

		private async Task AddNewImages(ProductCard sellerCard, EditProductCardViewModel productCardVM)
		{
			var images = new List<string> { sellerCard.Image.First, sellerCard.Image.Second, sellerCard.Image.Third, sellerCard.Image.Fourth, sellerCard.Image.Fifth };
			foreach (var image in productCardVM.Images)
			{
				var photoResult = await _photoService.AddPhotoAsync(image);
				var emptyImage = images.FirstOrDefault(i => string.IsNullOrEmpty(i));
				if (emptyImage != null)
					emptyImage = photoResult.Url.ToString();
			}
		}
	}
}
