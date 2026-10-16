using CareLink360.Domain.Entities;

public interface IJobRequestRepository
{
    Task<JobRequest> CreateAsync(JobRequest request);

    Task<JobRequest?> GetByIdAsync(Guid id);

    Task<List<JobRequest>> GetAllAsync();

    Task UpdateAsync(JobRequest request);
}