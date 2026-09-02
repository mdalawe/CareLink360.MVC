using CareLink360.Application.DTOs;
using CareLink360.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.Interfaces
{
    public interface IContractService
    {
        Task<Contract> CreateAsync(CreateContractDto dto);
        Task UpdateStatusAsync(UpdateContractStatusDto dto);
        Task<List<Contract>> GetAllAsync();
    }
}
