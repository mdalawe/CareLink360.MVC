using CareLink360.Context;
using CareLink360.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository(ApplicationDbContext context) : IPaymentRepository
    {
        public async Task<Payment> CreateAsync(Payment payment)
        {
            await context.Payments.AddAsync(payment);
            await context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> GetByIdAsync(Guid id)
        {
            return await context.Payments.FindAsync(id);
        }

        public async Task<List<Payment>> GetAllAsync()
        {
            return await context.Payments.ToListAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            context.Payments.Update(payment);
            await context.SaveChangesAsync();
        }
    }

