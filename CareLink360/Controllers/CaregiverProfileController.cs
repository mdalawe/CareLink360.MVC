using System.Security.Claims;
using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Web.ViewModels.Caregiver;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink360.Web.Controllers
{
    [Authorize(Roles = "Caregiver")]
    public class CaregiverProfileController : Controller
    {
        private readonly ICaregiverService _caregiverService;

        public CaregiverProfileController(
            ICaregiverService caregiverService)
        {
            _caregiverService = caregiverService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userIdString =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Auth");

            if (!Guid.TryParse(userIdString, out Guid userId))
                return RedirectToAction("Login", "Auth");

            var existingCaregiver =
                await _caregiverService.GetByUserIdAsync(userId);

            if (existingCaregiver != null)
            {
                return RedirectToAction(
                    "Index",
                    "CaregiverDashboard");
            }

            var model = new CreateCaregiverProfileViewModel
            {
                FirstName =
                    User.Identity?.Name ?? string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateCaregiverProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userIdString =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Auth");

            if (!Guid.TryParse(userIdString, out Guid userId))
                return RedirectToAction("Login", "Auth");

            var existingCaregiver =
                await _caregiverService.GetByUserIdAsync(userId);

            if (existingCaregiver != null)
            {
                return RedirectToAction(
                    "Index",
                    "CaregiverDashboard");
            }

            var dto = new CreateCaregiverProfileDto
            {
                UserId = userId,

                FirstName = model.FirstName,

                LastName = model.LastName,

                PhoneNumber = model.PhoneNumber,

                DateOfBirth = model.DateOfBirth,

                Gender = model.Gender,

                Address = model.Address,

                City = model.City,

                YearsOfExperience =
                    model.YearsOfExperience,

                Skills = model.Skills
            };

            await _caregiverService.CreateProfileAsync(dto);

            return RedirectToAction(
                "Index",
                "CaregiverDashboard");
        }


        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Auth");

            if (!Guid.TryParse(userIdString, out Guid userId))
                return RedirectToAction("Login", "Auth");

            var caregiver = await _caregiverService.GetByUserIdAsync(userId);

            if (caregiver == null)
            {
                return RedirectToAction(nameof(Create));
            }

            return View(caregiver);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAvailability()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Auth");

            if (!Guid.TryParse(userIdString, out Guid userId))
                return RedirectToAction("Login", "Auth");

            var success = await _caregiverService.ToggleAvailabilityAsync(userId);

            if (!success)
            {
                TempData["Error"] = "Caregiver profile not found.";
            }
            else
            {
                TempData["Success"] = "Availability updated!";
            }

            return RedirectToAction(nameof(MyProfile));
        }
    }
}