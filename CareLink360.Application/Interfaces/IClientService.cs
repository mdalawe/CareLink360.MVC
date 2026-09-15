using CareLink360.Application.DTOs;
using CareLink360.Domain.Entities;

public interface IClientService
{
    Task<Client> CreateAsync(CreateClientDto dto);
    Task<List<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id,CreateClientDto dto);
    Task<bool> DeleteAsync(Guid id);
}