using CareLink360.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.Interfaces
{
    public interface ICaregiverService
    {
        Task<CaregiverResponseDto> CreateAsync(CreateCaregiverDto dto);
        Task<List<CaregiverResponseDto>> GetAllAsync();
        Task<List<CaregiverResponseDto>> GetApprovedAvailableAsync();
        Task<CaregiverResponseDto?> GetByIdAsync(Guid id);
        Task<CaregiverResponseDto?> GetByUserIdAsync(Guid userId);
        Task<CaregiverResponseDto?> UpdateAsync(Guid id, UpdateCaregiverDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<CaregiverResponseDto> CreateProfileAsync(CreateCaregiverProfileDto dto);
        Task<bool> ToggleAvailabilityAsync(Guid userId);
        Task<bool> SetAvailabilityAsync(Guid caregiverId, bool isAvailable);
        Task<bool> SetApprovalStatusAsync(Guid id, bool isApproved);
    }
}
