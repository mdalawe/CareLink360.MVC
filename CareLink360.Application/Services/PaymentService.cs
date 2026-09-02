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
            Amount = dto.Amount
        };

        return await repository.CreateAsync(payment);
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
}