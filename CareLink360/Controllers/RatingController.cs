using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/ratings")]
public class RatingController(IRatingService service) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateRatingDto dto)
    {
        return Ok(await service.CreateAsync(dto));
    }

    [Authorize]
    [HttpGet("{caregiverId}")]
    public async Task<IActionResult> GetByCaregiver(Guid caregiverId)
    {
        return Ok(await service.GetByCaregiverIdAsync(caregiverId));
    }
}