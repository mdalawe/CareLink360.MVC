
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CareLink360.Context;
using CareLink360.Web.Models;
using CareLink360.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareLink360.Web.Controllers
{
    [Authorize(Roles = "Client,Patient")]
    public class ClientProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
            {
                return NotFound("Your client profile could not be found.");
            }

            var model = new ClientProfileViewModel
            {
                Id = client.Id,
                FullName = client.FullName,
                Email = User.FindFirstValue(ClaimTypes.Email) ?? "",
                PhoneNumber = client.PhoneNumber,
                HomeAddress = client.HomeAddress,
                City = client.City
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userIdString = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
            {
                return NotFound("Your client profile could not be found.");
            }

            var model = new ClientProfileViewModel
            {
                Id = client.Id,
                FullName = client.FullName,
                Email = User.FindFirstValue(ClaimTypes.Email) ?? "",
                PhoneNumber = client.PhoneNumber,
                HomeAddress = client.HomeAddress,
                City = client.City
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClientProfileViewModel model)
        {
            var userIdString = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
            {
                return NotFound("Your client profile could not be found.");
            }

            if (!ModelState.IsValid)
            {
                model.Id = client.Id;
                model.Email = User.FindFirstValue(ClaimTypes.Email) ?? "";

                return View(model);
            }

            client.FullName = model.FullName;
            client.PhoneNumber = model.PhoneNumber;
            client.HomeAddress = model.HomeAddress;
            client.City = model.City;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Your profile has been updated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
