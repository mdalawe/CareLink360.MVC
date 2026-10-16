using CareLink360.Domain.Entities;

public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> GetByIdAsync(Guid id);
        Task<List<Payment>> GetAllAsync();
    Task<List<Payment>> GetByClientIdAsync(Guid clientId);
        Task UpdateAsync(Payment payment);
    }

