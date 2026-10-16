using System.Security.Claims;
using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink360.Web.Controllers
{
    [Authorize]
    public class PaymentController(
        IPaymentService service,
        IClientService clientService) : Controller
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var payments = await service.GetAllAsync();

            return View(payments);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var payment = await service.GetByIdAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var payment = await service.GetByIdAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            var dto = new UpdatePaymentStatusDto
            {
                PaymentId = payment.Id,
                Status = payment.Status
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            UpdatePaymentStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await service.UpdateStatusAsync(dto);

                TempData["Success"] = "Payment status updated!";

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
        public async Task<IActionResult> MyPayments()
        {
            var userIdString =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client =
                await clientService.GetByUserIdAsync(userId);

            if (client == null)
            {
                return NotFound("Client profile not found.");
            }

            var payments =
                await service.GetByClientIdAsync(client.Id);

            return View(payments);
        }
    }
}