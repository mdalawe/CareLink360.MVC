using Microsoft.AspNetCore.Mvc;
using CareLink360.Web.Models;

namespace CareLink360.Web.Controllers
{
    public class HomeController : Controller
    {
        // Home Page
        public IActionResult Index()
        {
            return View();
        }

        // Privacy Page
        public IActionResult Privacy()
        {
            return View();
        }

   
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // TODO: Call login service/API

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // TODO: Call register service/API

            TempData["Success"] = "Account created successfully.";

            return RedirectToAction(nameof(Login));
        }
    }
}