using CareLink360.Context;
using CareLink360.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareLink360.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminDashboardViewModel
            {
                TotalCaregivers = await _context.Caregivers.CountAsync(),

                TotalClients = await _context.Clients.CountAsync(),

                TotalJobRequests = await _context.JobRequests.CountAsync(),

                TotalPayments = await _context.Payments.CountAsync(),

                TotalContracts = await _context.Contracts.CountAsync(),

                TotalMedicalRecords = await _context.MedicalRecords.CountAsync(),

                TotalRatings = await _context.Ratings.CountAsync(),

                PendingNinVerifications =
                    await _context.NinVerifications
                        .CountAsync(x => x.Status == "Pending"),

                ActiveCaregivers =
                    await _context.Caregivers
                        .CountAsync(x => x.IsApprovedByAdmin),

                PendingCaregivers =
                    await _context.Caregivers
                        .CountAsync(x => !x.IsApprovedByAdmin),

                AverageRating =
                    await _context.Caregivers
                        .Select(x => (decimal?)x.AverageRating)
                        .AverageAsync() ?? 0
            };

            return View(model);
        }
    }
}