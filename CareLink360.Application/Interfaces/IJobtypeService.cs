using CareLink360.Domain.Entities;

namespace CareLink360.Application.Interfaces
{
    public interface IJobTypeService
    {
        Task<List<JobType>> GetActiveJobTypesAsync();
    }
}