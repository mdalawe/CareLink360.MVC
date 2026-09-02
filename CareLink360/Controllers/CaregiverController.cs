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

        public CaregiverController(ICaregiverService caregiverService)
        {
            _caregiverService = caregiverService;
        }

        // GET: /Caregiver
        public async Task<IActionResult> Index()
        {
            var caregivers = await _caregiverService.GetAllAsync();

            return View(caregivers);
        }

        // GET: /Caregiver/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateCaregiverDto());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCaregiverDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _caregiverService.CreateAsync(dto);

                TempData["Success"] = "Caregiver created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to create caregiver: " + ex.Message
                );

                return View(dto);
            }
        }

        // GET: /Caregiver/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var caregiver = await _caregiverService.GetByIdAsync(id);

            if (caregiver == null)
            {
                return NotFound();
            }

            var model = new UpdateCaregiverDto
            {
                PhoneNumber = caregiver.PhoneNumber,
                Address = "",
                City = caregiver.City,
                YearsOfExperience = caregiver.YearsOfExperience,
                Skills = "",
                IsAvaliable = caregiver.IsAvaliable
            };

            ViewBag.CaregiverId = caregiver.Id;
            ViewBag.CaregiverName =
                $"{caregiver.FirstName} {caregiver.LastName}";

            return View(model);
        }

        // POST: /Caregiver/Edit/{id}
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
                var result = await _caregiverService.UpdateAsync(id, dto);

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
                    "",
                    "Unable to update caregiver: " + ex.Message
                );

                ViewBag.CaregiverId = id;

                return View(dto);
            }
        }

        // GET: /Caregiver/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var caregiver = await _caregiverService.GetByIdAsync(id);

            if (caregiver == null)
            {
                return NotFound();
            }

            return View(caregiver);
        }

        // GET: /Caregiver/Delete/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var caregiver = await _caregiverService.GetByIdAsync(id);

            if (caregiver == null)
            {
                return NotFound();
            }

            return View(caregiver);
        }

        // POST: /Caregiver/Delete/{id}
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _caregiverService.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            TempData["Success"] =
                "Caregiver deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}