using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class ContractController(
    IContractService service
) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateContractDto dto)
    {
        return Ok(await service.CreateAsync(dto));
    }

    [Authorize]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(UpdateContractStatusDto dto)
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