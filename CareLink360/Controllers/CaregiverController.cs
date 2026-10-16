
using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink360.Web.Controllers
{
    [Authorize]
    public class CaregiverController : Controller
    {
        private readonly ICaregiverService _caregiverService;

        public CaregiverController(
            ICaregiverService caregiverService)
        {
            _caregiverService = caregiverService;
        }

        // ==========================================
        // ADMIN: VIEW ALL CAREGIVERS
        // URL: /Caregiver/Index
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var caregivers =
                await _caregiverService.GetAllAsync();

            return View(caregivers);
        }

        // ==========================================
        // ADMIN: DISPLAY CREATE FORM
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateCaregiverDto());
        }

        // ==========================================
        // ADMIN: CREATE CAREGIVER
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateCaregiverDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _caregiverService.CreateAsync(dto);

                TempData["Success"] =
                    "Caregiver created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create caregiver: " + ex.Message);

                return View(dto);
            }
        }

        // ==========================================
        // ADMIN / CAREGIVER: DISPLAY EDIT FORM
        // ==========================================
        [Authorize(Roles = "Admin,Caregiver")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var caregiver =
                await _caregiverService.GetByIdAsync(id);

            if (caregiver == null)
            {
                return NotFound();
            }

            var model = new UpdateCaregiverDto
            {
                PhoneNumber = caregiver.PhoneNumber,
                Address = "",
                City = caregiver.City,
                YearsOfExperience =
                    caregiver.YearsOfExperience,
                Skills = "",
                IsAvaliable = caregiver.IsAvaliable
            };

            ViewBag.CaregiverId = caregiver.Id;

            ViewBag.CaregiverName =
                $"{caregiver.FirstName} {caregiver.LastName}";

            return View(model);
        }

        // ==========================================
        // ADMIN / CAREGIVER: UPDATE CAREGIVER
        // ==========================================
        [Authorize(Roles = "Admin,Caregiver")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            UpdateCaregiverDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CaregiverId = id;
                return View(dto);
            }

            try
            {
                var result =
                    await _caregiverService.UpdateAsync(id, dto);

                if (result == null)
                {
                    return NotFound();
                }

                TempData["Success"] =
                    "Caregiver updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to update caregiver: " + ex.Message);

                ViewBag.CaregiverId = id;

                return View(dto);
            }
        }

        // ==========================================
        // VIEW CAREGIVER DETAILS
        // ==========================================
        [Authorize(Roles = "Admin,Caregiver,Patient,Client")]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var caregiver =
                await _caregiverService.GetByIdAsync(id);

            if (caregiver == null)
            {
                return NotFound();
            }

            return View(caregiver);
        }

        // ==========================================
        // ADMIN: DISPLAY DELETE CONFIRMATION
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var caregiver =
                await _caregiverService.GetByIdAsync(id);

            if (caregiver == null)
            {
                return NotFound();
            }

            return View(caregiver);
        }

        // ==========================================
        // ADMIN: DELETE CAREGIVER
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            Guid id)
        {
            var result =
                await _caregiverService.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            TempData["Success"] =
                "Caregiver deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // CLIENT / PATIENT: BROWSE CAREGIVERS
        // Only approved and available caregivers
        // ==========================================
        [Authorize(Roles = "Patient,Client")]
        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            var caregivers =
                await _caregiverService
                    .GetApprovedAvailableAsync();

            return View(caregivers);
        }

        // ==========================================
        // ADMIN: CHANGE CAREGIVER AVAILABILITY
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetAvailability(
            Guid id,
            bool isAvailable)
        {
            try
            {
                var updated =
                    await _caregiverService
                        .SetAvailabilityAsync(id, isAvailable);

                if (!updated)
                {
                    TempData["Error"] =
                        "Caregiver not found.";

                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = isAvailable
                    ? "Caregiver is now available."
                    : "Caregiver is now unavailable.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // ADMIN: APPROVE OR REVOKE CAREGIVER APPROVAL
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetApprovalStatus(
            Guid id,
            bool isApproved)
        {
            try
            {
                var updated =
                    await _caregiverService
                        .SetApprovalStatusAsync(id, isApproved);

                if (!updated)
                {
                    TempData["Error"] =
                        "Caregiver not found.";

                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] = isApproved
                    ? "Caregiver approved successfully."
                    : "Caregiver approval has been revoked.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
