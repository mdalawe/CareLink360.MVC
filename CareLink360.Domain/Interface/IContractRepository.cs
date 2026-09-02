using CareLink360.Domain.Entities;

public interface IContractRepository
{
    Task<Contract> CreateAsync(Contract contract);
    Task<Contract?> GetByIdAsync(Guid id);
    Task<List<Contract>> GetAllAsync();
    Task UpdateAsync(Contract contract);
}