using CareLink360.Context;
using CareLink360.Domain.Entities;
using CareLink360.Domain.Interface;
using Microsoft.EntityFrameworkCore;

public class CaregiverRepository(ApplicationDbContext context) : ICaregiverRepository
{
    public async Task<Caregiver> CreateAsync(Caregiver caregiver)
    {
        await context.Caregivers.AddAsync(caregiver);
        await context.SaveChangesAsync();
        return caregiver;
    }

    public async Task<Caregiver?> GetByUserIdAsync(Guid userId)
    {
        return await context.Caregivers
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<List<Caregiver>> GetAllAsync()
    {
        return await context.Caregivers.ToListAsync();
    }

    public async Task<Caregiver?> GetByIdAsync(Guid id)
    {
        return await context.Caregivers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Caregiver?> UpdateAsync(Caregiver caregiver)
    {
        context.Caregivers.Update(caregiver);
        await context.SaveChangesAsync();
        return caregiver;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var caregiver = await context.Caregivers.FindAsync(id);

        if (caregiver == null)
            return false;

        context.Caregivers.Remove(caregiver);
        await context.SaveChangesAsync();

        return true;
    }
}