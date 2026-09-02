using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/medical")]
public class MedicalController(IMedicalService service) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateMedicalRecordDto dto)
    {
        return Ok(await service.CreateAsync(dto));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("clearance")]
    public async Task<IActionResult> UpdateClearance(UpdateMedicalClearanceDto dto)
    {
        await service.UpdateClearanceAsync(dto);
        return Ok("Updated");
    }

    [Authorize]
    [HttpGet("caregiver/{id}")]
    public async Task<IActionResult> GetByCaregiver(Guid id)
    {
        return Ok(await service.GetByCaregiverIdAsync(id));
    }
}