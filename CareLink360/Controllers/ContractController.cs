using System.Security.Claims;
using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink360.Web.Controllers
{
    [Authorize]
    public class ContractController : Controller
    {
        private readonly IContractService _service;
        private readonly IClientService _clientService;

        public ContractController(
            IContractService service,
            IClientService clientService)
        {
            _service = service;
            _clientService = clientService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contracts = await _service.GetAllAsync();

            return View(contracts);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateContractDto();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateContractDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _service.CreateAsync(dto);

                TempData["Success"] =
                    "Contract created successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(dto);
            }
        }


        [HttpGet]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var contract =
                await _service.GetByIdAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            var dto = new UpdateContractStatusDto
            {
                ContractId = contract.Id,
                Status = contract.Status
            };

            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            UpdateContractStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _service.UpdateStatusAsync(dto);

                TempData["Success"] =
                    "Contract status updated!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                return View(dto);
            }
        }


        [Authorize(Roles = "Client,Patient")]
        [HttpGet]
        public async Task<IActionResult> MyContracts()
        {
            var userIdString =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(
                    userIdString,
                    out Guid userId))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }
            var client =
                await _clientService
                    .GetByUserIdAsync(userId);

            if (client == null)
            {
                return NotFound(
                    "Client profile not found.");
            }

            var contracts =
                await _service
                    .GetByClientIdAsync(client.Id);

            return View(contracts);
        }
    }
}