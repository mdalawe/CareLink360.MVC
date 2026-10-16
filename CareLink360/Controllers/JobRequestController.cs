using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareLink360.Web.Controllers
{
    [Authorize]
    public class JobRequestController : Controller
    {
        private readonly IJobRequestService _service;
        private readonly IJobTypeService _jobTypeService;
        private readonly ICaregiverService _caregiverService;
        private readonly IClientService _clientService;

        public JobRequestController(
            IJobRequestService service,
            IJobTypeService jobTypeService,
            ICaregiverService caregiverService,
            IClientService clientService)
        {
            _service = service;
            _jobTypeService = jobTypeService;
            _caregiverService = caregiverService;
            _clientService = clientService;
        }

        // ADMIN: VIEW ALL JOB REQUESTS
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var requests = await _service.GetAllAsync();
            return View(requests);
        }

        // CLIENT: OPEN CREATE REQUEST PAGE
        [Authorize(Roles = "Client,Patient")]
        [HttpGet]
        public async Task<IActionResult> Create(Guid? caregiverId)
        {
            await LoadCreateDropdowns();

            var model = new CreateJobRequestDto
            {
                CaregiverId = caregiverId ?? Guid.Empty
            };

            if (model.CaregiverId != Guid.Empty)
            {
                var caregivers =
                    await _caregiverService.GetApprovedAvailableAsync();

                var caregiverExists = caregivers.Any(
                    c => c.Id == model.CaregiverId);

                if (!caregiverExists)
                {
                    ModelState.AddModelError(
                        nameof(model.CaregiverId),
                        "This caregiver is not currently approved and available.");

                    model.CaregiverId = Guid.Empty;
                }
            }

            return View(model);
        }

        // CLIENT: SUBMIT JOB REQUEST
        [Authorize(Roles = "Client,Patient")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateJobRequestDto dto)
        {
            var userIdString =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client =
                await _clientService.GetByUserIdAsync(userId);

            if (client == null)
            {
                return NotFound("Client profile not found.");
            }

            dto.ClientId = client.Id;

            // Confirm the caregiver is approved and available.
            var availableCaregivers =
                await _caregiverService.GetApprovedAvailableAsync();

            var selectedCaregiver = availableCaregivers.FirstOrDefault(
                c => c.Id == dto.CaregiverId);

            if (selectedCaregiver == null)
            {
                ModelState.AddModelError(
                    nameof(dto.CaregiverId),
                    "The selected caregiver is no longer approved and available.");
            }

            // Confirm the selected job type is active.
            var activeJobTypes =
                await _jobTypeService.GetActiveJobTypesAsync();

            var selectedJobType = activeJobTypes.FirstOrDefault(
                j => j.Id == dto.JobTypeId);

            if (selectedJobType == null)
            {
                ModelState.AddModelError(
                    nameof(dto.JobTypeId),
                    "Please select a valid type of care.");
            }

            if (!ModelState.IsValid)
            {
                await LoadCreateDropdowns();
                return View(dto);
            }

            try
            {
                await _service.CreateAsync(dto);

                TempData["Success"] =
                    "Your caregiver request has been sent successfully.";

                return RedirectToAction(nameof(MyCreatedRequests));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Your request could not be created: " + ex.Message);

                await LoadCreateDropdowns();
                return View(dto);
            }
        }

        // CAREGIVER: ACCEPT A JOB REQUEST
        [Authorize(Roles = "Caregiver")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(Guid id)
        {
            try
            {
                var userIdString =
                    User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!Guid.TryParse(userIdString, out Guid userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var caregiver =
                    await _caregiverService.GetByUserIdAsync(userId);

                if (caregiver == null)
                {
                    return NotFound("Caregiver profile not found.");
                }

                var request = await _service.GetByIdAsync(id);

                if (request == null)
                {
                    return NotFound();
                }

                if (request.CaregiverId != caregiver.Id)
                {
                    return Forbid();
                }

                await _service.AcceptAsync(id);

                TempData["Success"] =
                    "Job request accepted — contract created!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(MyRequests));
        }

        // CAREGIVER: REJECT A JOB REQUEST
        [Authorize(Roles = "Caregiver")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id)
        {
            try
            {
                var userIdString =
                    User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!Guid.TryParse(userIdString, out Guid userId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var caregiver =
                    await _caregiverService.GetByUserIdAsync(userId);

                if (caregiver == null)
                {
                    return NotFound("Caregiver profile not found.");
                }

                var request = await _service.GetByIdAsync(id);

                if (request == null)
                {
                    return NotFound();
                }

                if (request.CaregiverId != caregiver.Id)
                {
                    return Forbid();
                }

                await _service.RejectAsync(id);

                TempData["Success"] = "Job request rejected.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(MyRequests));
        }

        // CLIENT: VIEW THEIR CREATED REQUESTS
        [Authorize(Roles = "Client,Patient")]
        [HttpGet]
        public async Task<IActionResult> MyCreatedRequests()
        {
            var userIdString =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client =
                await _clientService.GetByUserIdAsync(userId);

            if (client == null)
            {
                return NotFound("Client profile not found.");
            }

            var allRequests = await _service.GetAllAsync();

            var myRequests = allRequests
                .Where(x => x.ClientId == client.Id)
                .ToList();

            return View("MyCreatedRequests", myRequests);
        }

        // CAREGIVER: VIEW REQUESTS SENT TO THEM
        [Authorize(Roles = "Caregiver")]
        [HttpGet]
        public async Task<IActionResult> MyRequests()
        {
            var userIdString =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var caregiver =
                await _caregiverService.GetByUserIdAsync(userId);

            if (caregiver == null)
            {
                return NotFound("Caregiver profile not found.");
            }

            var allRequests = await _service.GetAllAsync();

            var myRequests = allRequests
                .Where(x => x.CaregiverId == caregiver.Id)
                .ToList();

            return View(myRequests);
        }

        // LOAD THE DATA NEEDED BY THE CREATE VIEW
        private async Task LoadCreateDropdowns()
        {
            ViewBag.JobTypes =
                await _jobTypeService.GetActiveJobTypesAsync();

            ViewBag.Caregivers =
                await _caregiverService.GetApprovedAvailableAsync();
        }
    }
}
