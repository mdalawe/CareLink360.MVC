using CareLink360.Application.DTOs;
using CareLink360.Domain.Entities;

namespace CareLink360.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApplicationUser?> RegisterAsync(RegisterRequestDto request);

        Task<ApplicationUser?> LoginAsync(LoginRequest request);
    }
}