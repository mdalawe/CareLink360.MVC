using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Domain.Entities;
using CareLink360.Domain.Enums;

public class ContractService(IContractRepository repository) : IContractService
{
    public async Task<Contract> CreateAsync(CreateContractDto dto)
    {
        var contract = new Contract
        {
            ClientId = dto.ClientId,
            CaregiverId = dto.CaregiverId,
            JobRequestId = dto.JobRequestId
        };

        return await repository.CreateAsync(contract);
    }


    public async Task<Contract?> GetByIdAsync(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task UpdateStatusAsync(UpdateContractStatusDto dto)
    {
        var contract = await repository.GetByIdAsync(dto.ContractId);

        if (contract == null)
            throw new Exception("Contract not found");

        contract.Status = dto.Status;

        if (dto.Status == ContractStatus.Successful || dto.Status == ContractStatus.Cancelled)
        {
            contract.EndDate = DateTime.UtcNow;
        }

        await repository.UpdateAsync(contract);
    }


    public async Task<List<Contract>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }
}