using System.Security.Claims;
using AppointmentApp.Models;
using AppointmentApp.Services;
using AppointmentApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiClient _api;

        public AccountController(IApiClient api) => _api = api;

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var ok = await _api.RegisterAsync(new RegistrationDTO
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                Role = "patient"
            });

            if (!ok)
            {
                ModelState.AddModelError("", "Registration failed. That email may already be in use.");
                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var role = await _api.LoginAsync(new LoginDTO
            {
                Email = model.Email,
                Password = model.Password
            });

            if (role is null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return role == "staff"
                ? RedirectToAction("Dashboard", "Staff")
                : RedirectToAction("Dashboard", "Patient");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}