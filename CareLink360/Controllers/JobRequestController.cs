using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/job-requests")]
public class JobRequestController(IJobRequestService service) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateJobRequestDto dto)
    {
        return Ok(await service.CreateAsync(dto));
    }

    [Authorize]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(UpdateJobRequestDto dto)
    {
        await service.UpdateStatusAsync(dto);
        return Ok("Updated");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await service.GetAllAsync());
    }
}