using CareLink360.Context;
using CareLink360.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareLink360.Infrastructure.Repository;

public class MedicalRepository : IMedicalRepository
{
    private readonly ApplicationDbContext _context;

    public MedicalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MedicalRecord> CreateAsync(MedicalRecord record)
    {
        await _context.MedicalRecords.AddAsync(record);
        await _context.SaveChangesAsync();

        return record;
    }

    public async Task<MedicalRecord?> GetByIdAsync(Guid id)
    {
        return await _context.MedicalRecords.FindAsync(id);
    }

    public async Task<List<MedicalRecord>> GetByCaregiverIdAsync(Guid caregiverId)
    {
        return await _context.MedicalRecords
            .Where(x => x.CaregiverId == caregiverId)
            .ToListAsync();
    }

    public async Task UpdateAsync(MedicalRecord record)
    {
        _context.MedicalRecords.Update(record);
        await _context.SaveChangesAsync();
    }
}