using System.Security.Claims;
using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Context;
using CareLink360.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareLink360.Web.Controllers
{
    [Authorize]
    public class ContractController : Controller
    {
        private readonly IContractService _service;
        private readonly ApplicationDbContext _context;

        public ContractController(
            IContractService service,
            ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        // ADMIN: View all contracts.
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contracts = await _service.GetAllAsync();
            return View(contracts);
        }

        // ADMIN ONLY: Manual creation is restricted.
        // Normal contracts should be created when a caregiver accepts.
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateContractDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateContractDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _service.CreateAsync(dto);

                TempData["Success"] =
                    "Contract created successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty, ex.Message);

                return View(dto);
            }
        }

        // CLIENT AND CAREGIVER: View only their own contracts.
        [Authorize(Roles = "Client,Patient,Caregiver")]
        [HttpGet]
        public async Task<IActionResult> MyContracts()
        {
            var userIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
                return Challenge();

            IQueryable<Contract> query = _context.Contracts
                .AsNoTracking()
                .Include(c => c.Client)
                .Include(c => c.Caregiver)
                .Include(c => c.JobRequest);

            if (User.IsInRole("Client") ||
                User.IsInRole("Patient"))
            {
                var clientId = await _context.Clients
                    .Where(c => c.UserId == userId)
                    .Select(c => (Guid?)c.Id)
                    .FirstOrDefaultAsync();

                if (clientId == null)
                    return NotFound("Client profile not found.");

                query = query.Where(
                    c => c.ClientId == clientId.Value);
            }
            else if (User.IsInRole("Caregiver"))
            {
                var caregiverId = await _context.Caregivers
                    .Where(c => c.UserId == userId)
                    .Select(c => (Guid?)c.Id)
                    .FirstOrDefaultAsync();

                if (caregiverId == null)
                    return NotFound("Caregiver profile not found.");

                query = query.Where(
                    c => c.CaregiverId == caregiverId.Value);
            }
            else
            {
                return Forbid();
            }

            var contracts = await query
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(contracts);
        }

        // A specific contract is visible only to its participants
        // or an Admin.
        [Authorize(Roles = "Admin,Client,Patient,Caregiver")]
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var contract = await _context.Contracts
                .AsNoTracking()
                .Include(c => c.Client)
                .Include(c => c.Caregiver)
                .Include(c => c.JobRequest)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null)
                return NotFound();

            if (User.IsInRole("Admin"))
                return View(contract);

            var userIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
                return Challenge();

            bool isParticipant = false;

            if (User.IsInRole("Client") ||
                User.IsInRole("Patient"))
            {
                isParticipant = await _context.Clients
                    .AnyAsync(c =>
                        c.Id == contract.ClientId &&
                        c.UserId == userId);
            }

            if (User.IsInRole("Caregiver"))
            {
                isParticipant = await _context.Caregivers
                    .AnyAsync(c =>
                        c.Id == contract.CaregiverId &&
                        c.UserId == userId);
            }

            if (!isParticipant)
                return NotFound();

            return View(contract);
        }

        // Only Admin can change a contract's status.
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var contract = await _service.GetByIdAsync(id);

            if (contract == null)
                return NotFound();

            var dto = new UpdateContractStatusDto
            {
                ContractId = contract.Id,
                Status = contract.Status
            };

            return View(dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            UpdateContractStatusDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _service.UpdateStatusAsync(dto);

                TempData["Success"] =
                    "Contract status updated successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty, ex.Message);

                return View(dto);
            }
        }
    }
}