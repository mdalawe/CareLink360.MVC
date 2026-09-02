using CareLink360.Context;
using CareLink360.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareLink360.Infrastructure.Repository;

public class ContractRepository : IContractRepository
{
    private readonly ApplicationDbContext _context;

    public ContractRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Contract> CreateAsync(Contract contract)
    {
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        return contract;
    }

    public async Task<Contract?> GetByIdAsync(Guid id)
    {
        return await _context.Contracts.FindAsync(id);
    }

    public async Task<List<Contract>> GetAllAsync()
    {
        return await _context.Contracts.ToListAsync();
    }

    public async Task UpdateAsync(Contract contract)
    {
        _context.Contracts.Update(contract);
        await _context.SaveChangesAsync();
    }
}