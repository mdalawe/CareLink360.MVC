using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink360.Web.Controllers
{
    [Authorize]
    public class JobRequestController : Controller
    {
        private readonly IJobRequestService _service;

        public JobRequestController(IJobRequestService service)
        {
            _service = service;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var requests = await _service.GetAllAsync();

            return View(requests);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateJobRequestDto();

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _service.CreateAsync(dto);

                TempData["Success"] = "Job request sent!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return View(dto);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(Guid id)
        {
            try
            {
                await _service.AcceptAsync(id);

                TempData["Success"] = "Job request accepted — contract created!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(Guid id)
        {
            try
            {
                await _service.RejectAsync(id);

                TempData["Success"] = "Job request rejected.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}