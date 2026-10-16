using CareLink360.Application.DTOs;
using CareLink360.Domain.Entities;

namespace CareLink360.Application.Interfaces
{
    public interface IContractService
    {
        Task<Contract> CreateAsync(CreateContractDto dto);

        Task<Contract?> GetByIdAsync(Guid id);

        Task UpdateStatusAsync(UpdateContractStatusDto dto);

        Task<List<Contract>> GetAllAsync();

        Task<List<Contract>> GetByClientIdAsync(Guid clientId);
    }
}