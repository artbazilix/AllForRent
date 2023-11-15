using AllForRent.Data;
using AllForRent.Models;
using AllForRent.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AllForRent.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);
            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["Error"] = "Неверные данные, попробуйте еще раз";
                return View(loginViewModel);
            }
            TempData["Error"] = "Неверные данные, попробуйте еще раз";
            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            var response = new RegistrationViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel registrationViewModel)
        {
            if (!ModelState.IsValid) return View(registrationViewModel);

            var user = await _userManager.FindByEmailAsync(registrationViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "Этот email уже зарегистрирован";
                return View(registrationViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registrationViewModel.EmailAddress,
                UserName = registrationViewModel.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registrationViewModel.Password);

            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}