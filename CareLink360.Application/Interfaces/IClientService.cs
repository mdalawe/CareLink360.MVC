
using CareLink360.Application.DTOs;
using CareLink360.Domain.Entities;

namespace CareLink360.Application.Interfaces
{
    public interface IClientService
    {
        Task<Client> CreateAsync(CreateClientDto dto);

        Task<List<Client>> GetAllAsync();

        Task<Client?> GetByIdAsync(Guid id);

        Task<bool> UpdateAsync(Guid id, UpdateClientDto dto);

        Task<Client?> GetByUserIdAsync(Guid userId);

        Task<bool> DeleteAsync(Guid id);
    }
}
