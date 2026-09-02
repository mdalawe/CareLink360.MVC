using CareLink360.Application.DTOs;
using CareLink360.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CareLink360.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> CreateAsync(CreatePaymentDto dto);
        Task UpdateStatusAsync(UpdatePaymentStatusDto dto);
        Task<List<Payment>> GetAllAsync();
    }
}
