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
        Task<CaregiverResponseDto?> GetByIdAsync(Guid id);
        Task<CaregiverResponseDto?> GetByUserIdAsync(Guid userId);
        Task<CaregiverResponseDto?> UpdateAsync(Guid id, UpdateCaregiverDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
