using CareLink360.Domain.Entities;

namespace CareLink360.Domain.Interface
{
    public interface IClientRepository
    {
        Task<Client> CreateAsync(Client client);

        Task<List<Client>> GetAllAsync();

        Task UpdateAsync(Client client);

        Task<Client?> GetByIdAsync(Guid id);

        Task<Client?> GetByUserIdAsync(Guid userId);

        Task<bool> DeleteAsync(Guid id);
    }
}