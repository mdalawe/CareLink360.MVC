using System.Security.Claims;
using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareLink360.Api.Controllers
{
    [ApiController]
    [Route("api/nin")]
    [Authorize] 
    public class NinVerificationController : ControllerBase
    {
        private readonly INinVerificationService _service;

        public NinVerificationController(INinVerificationService service)
        {
            _service = service;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitNinDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Invalid token");

            dto.UserId = Guid.Parse(userIdClaim);

            await _service.SubmitAsync(dto);

            return Ok("NIN submitted successfully");
        }

        [HttpPost("review")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Review([FromBody] ReviewNinDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.ReviewAsync(dto);

            return Ok("Reviewed successfully");
        }
    }
}