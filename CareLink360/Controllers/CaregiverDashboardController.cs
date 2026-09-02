using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareLink360.Web.Controllers
{
    [Authorize]
    public class CaregiverDashboardController : Controller
    {
        private readonly ICaregiverService _caregiverService;

        public CaregiverDashboardController(
            ICaregiverService caregiverService)
        {
            _caregiverService = caregiverService;
        }

        // GET: /CaregiverDashboard
        public async Task<IActionResult> Index()
        {
            // Get the logged-in user's ID
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Find caregiver using the logged-in user's ID
            var caregiver = await _caregiverService.GetByUserIdAsync(userId);

            // Caregiver profile does not exist
            if (caregiver == null)
            {
                return View("NotFound");
            }

            // Send caregiver profile to dashboard
            return View(caregiver);
        }
    }
}