using System.Security.Claims;
using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CareLink360.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new RegisterRequestDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                Role = model.Role
            };

            var user = await _service.RegisterAsync(dto);

            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    "Registration failed. Email may already exist.");

                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var request = new LoginRequest
            {
                Email = model.Email,
                Password = model.Password
            };

            var user = await _service.LoginAsync(request);

            if (user == null)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid email or password.");

                return View(model);
            }


            // ==========================================
            // CREATE AUTHENTICATION CLAIMS
            // ==========================================

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.Email ?? string.Empty),

                new Claim(
                    ClaimTypes.Email,
                    user.Email ?? string.Empty),

                new Claim(
                    ClaimTypes.Role,
                    user.Role ?? string.Empty)
            };


            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);


            var principal = new ClaimsPrincipal(identity);


            // ==========================================
            // CREATE LOGIN COOKIE
            // ==========================================

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);


            // ==========================================
            // REDIRECT BASED ON ROLE
            // ==========================================

            if (user.Role == "Admin")
            {
                return RedirectToAction(
                    "Index",
                    "AdminDashboard");
            }

            if (user.Role == "Caregiver")
            {
                return RedirectToAction(
                    "Index",
                    "CaregiverDashboard");
            }

            if (user.Role == "Patient")
            {
                return RedirectToAction(
                    "Index",
                    "ClientDashboard");
            }


            return RedirectToAction(
                "Index",
                "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(
                "Index",
                "Home");
        }
    }
}