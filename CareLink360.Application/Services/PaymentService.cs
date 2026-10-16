using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Domain.Entities;

public class PaymentService(IPaymentRepository repository) : IPaymentService
{
    public async Task<Payment> CreateAsync(CreatePaymentDto dto)
    {
        var payment = new Payment
        {
            ContractId = dto.ContractId,
            ClientId = dto.ClientId,
            PaymentMethod = dto.PaymentMethod,
            Amount = dto.Amount
        };

        return await repository.CreateAsync(payment);
    }


    public async Task<Payment?> GetByIdAsync(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }


    public async Task UpdateStatusAsync(UpdatePaymentStatusDto dto)
    {
        var payment = await repository.GetByIdAsync(dto.PaymentId);

        if (payment == null)
            throw new Exception("Payment not found");

        payment.Status = dto.Status;
        await repository.UpdateAsync(payment);
    }


    public async Task<List<Payment>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<List<Payment>> GetByClientIdAsync(Guid clientId)
    {
        return await repository.GetByClientIdAsync(clientId);
    }
}