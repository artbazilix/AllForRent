using AllForRent.Interfaces;
using AllForRent.Models;
using AllForRent.ViewModels;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AllForRent.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRespository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IDashboardRepository dashboardRespository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _dashboardRespository = dashboardRespository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRespository.GetUserById(curUserId);
            var userProductCards = await _dashboardRespository.GetAllUserProductCards();

            var dashboardViewModel = new DashboardViewModel()
            {
                ProductCards = userProductCards,
                Balance = user.Balance
            };

            return View(dashboardViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRespository.GetUserById(curUserId);
            if (user == null) return View("Error");

            var editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                FullName = user.FullName,
                ProfileImageUrl = user.ProfileImageUrl,
                PhoneNumber = user.PhoneNumber,
            };

            if (user.Address != null)
            {
                editUserViewModel.City = user.Address.City;
                editUserViewModel.State = user.Address.State;
                editUserViewModel.Street = user.Address.Street;
            }

            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Ошибка редактирования");
                return View("EditUserProfile");
            }

            var user = await _dashboardRespository.GetByIdNoTracking(editVM.Id);

            if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null) 
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                
                MapUserEdit(user, editVM, photoResult);

                _dashboardRespository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try 
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Не удалось загрузить изображение");
                    return View(editVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult);
                _dashboardRespository.Update(user);
                return RedirectToAction("Index");
            }

        }

        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult photoResult)
        {
            user.Id = editVM.Id;
            user.FullName = editVM.FullName;
            user.PhoneNumber = editVM.PhoneNumber;

            if (photoResult != null && photoResult.Url != null)
            {
                user.ProfileImageUrl = photoResult.Url.ToString();
            }

            if (user.Address == null)
            {
                user.Address = new Address();
            }

            user.Address.Street = editVM.Street;
            user.Address.City = editVM.City;
            user.Address.State = editVM.State;
        }
    }
}
