using CareLink360.Domain.Entities;

public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> GetByIdAsync(Guid id);
        Task<List<Payment>> GetAllAsync();
        Task UpdateAsync(Payment payment);
    }

