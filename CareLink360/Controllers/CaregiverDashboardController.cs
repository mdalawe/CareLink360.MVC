using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink360.Web.Controllers
{
    [Authorize(Roles = "Caregiver")]
    public class CaregiverDashboardController : Controller
    {
        private readonly ICaregiverService _caregiverService;

        public CaregiverDashboardController(
            ICaregiverService caregiverService)
        {
            _caregiverService = caregiverService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdString =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var caregiver =
                await _caregiverService.GetByUserIdAsync(userId);

            if (caregiver == null)
            {
                return RedirectToAction(
                    "Create",
                    "CaregiverProfile");
            }

            return View(caregiver);
        }
    }
}