using CareLink360.Application.DTOs;
using CareLink360.Domain.Entities;

namespace CareLink360.Application.Interfaces
{
    public interface IJobRequestService
    {
        Task<JobRequest> CreateAsync(CreateJobRequestDto dto);

        Task<JobRequest?> GetByIdAsync(Guid id);

        Task<List<JobRequest>> GetAllAsync();

        Task AcceptAsync(Guid id);

        Task RejectAsync(Guid id);
    }
}