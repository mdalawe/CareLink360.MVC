using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink360.Web.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }


        // ==========================================
        // GET: /Client
        // SHOW ALL CLIENTS
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllAsync();

            return View(clients);
        }


        // ==========================================
        // GET: /Client/Create
        // SHOW CREATE CLIENT FORM
        // ==========================================

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateClientDto();

            return View(model);
        }


        // ==========================================
        // POST: /Client/Create
        // CREATE NEW CLIENT
        // ==========================================

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


        // ==========================================
        // GET: /Client/Details/{id}
        // SHOW ONE CLIENT
        // ==========================================

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


        // ==========================================
        // GET: /Client/Delete/{id}
        // SHOW DELETE CONFIRMATION
        // ==========================================

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


        // ==========================================
        // POST: /Client/Delete/{id}
        // DELETE CLIENT
        // ==========================================

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