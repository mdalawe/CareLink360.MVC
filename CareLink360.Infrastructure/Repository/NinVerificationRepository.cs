using CareLik360.Domain.Interface;
using CareLink360.Context;
using CareLink360.Domain.Entities;

namespace CareLink360.Infrastructure.Repository
{
    public class NinVerificationRepository(ApplicationDbContext context) : INinVerificationRepository
    {
        public async Task AddAsync(NinVerification verification)
        {
            await context.NinVerifications.AddAsync(verification);
            await context.SaveChangesAsync();
        }

        public async Task<NinVerification?> GetByIdAsync(Guid id)
        {
            return await context.NinVerifications.FindAsync(id);
        }

        public async Task UpdateAsync(NinVerification verification)
        {
            context.NinVerifications.Update(verification);
            await context.SaveChangesAsync();
        }

    }
}