using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareLink360.Web.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICaregiverService _caregiverService;

        public ClientController(
            IClientService clientService,
            ICaregiverService caregiverService)
        {
            _clientService = clientService;
            _caregiverService = caregiverService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllAsync();

            return View(clients);
        }



        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateClientDto();

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _clientService.CreateAsync(dto);

                TempData["Success"] =
                    "Client created successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;

                if (ex.InnerException != null)
                {
                    errorMessage +=
                        " | " + ex.InnerException.Message;
                }

                ModelState.AddModelError(
                    string.Empty,
                    errorMessage
                );

                return View(dto);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client =
                await _clientService.GetByIdAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            var dto = new CreateClientDto
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                PhoneNumber = client.PhoneNumber,
                City = client.City,
                Address = client.HomeAddress
            };

            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            CreateClientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _clientService.UpdateAsync(id, dto);

                TempData["Success"] =
                    "Client updated successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;

                if (ex.InnerException != null)
                {
                    errorMessage +=
                        " | " + ex.InnerException.Message;
                }

                ModelState.AddModelError(
                    string.Empty,
                    errorMessage
                );

                return View(dto);
            }
        }




        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var client =
                await _clientService.GetByIdAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client =
                await _clientService.GetByIdAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }



        [HttpGet]
        public async Task<IActionResult> Caregivers()
        {
            var caregivers =
                await _caregiverService
                    .GetApprovedAvailableAsync();

            return View(caregivers);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var result =
                    await _clientService.DeleteAsync(id);

                if (!result)
                {
                    TempData["Error"] =
                        "Client could not be deleted.";

                    return RedirectToAction(nameof(Index));
                }

                TempData["Success"] =
                    "Client deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    "Error deleting client: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}