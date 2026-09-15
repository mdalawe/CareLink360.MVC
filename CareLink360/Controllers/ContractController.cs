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

        public ContractController(IContractService service)
        {
            _service = service;
        }


        // ==========================================
        // GET: /Contract
        // SHOW ALL CONTRACTS
        // ==========================================

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contracts = await _service.GetAllAsync();

            return View(contracts);
        }


        // ==========================================
        // GET: /Contract/Create
        // SHOW CREATE CONTRACT FORM
        // ==========================================

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateContractDto();

            return View(model);
        }


        // ==========================================
        // POST: /Contract/Create
        // CREATE NEW CONTRACT
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateContractDto dto)
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
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(dto);
            }
        }


      

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var contract = await _service.GetByIdAsync(id);

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
        public async Task<IActionResult> UpdateStatus(UpdateContractStatusDto dto)
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
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(dto);
            }
        }
    }
}