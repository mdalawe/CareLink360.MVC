
using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Context;
using CareLink360.Domain.Entities;
using CareLink360.Domain.Enums;
using CareLink360.Domain.Interface;
using Microsoft.EntityFrameworkCore;

public class ContractService : IContractService
{
    private readonly IContractRepository _repository;
    private readonly ApplicationDbContext _context;

    public ContractService(
        IContractRepository repository,
        ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<Contract> CreateAsync(
        CreateContractDto dto)
    {
        var caregiver = await _context.Caregivers
            .FirstOrDefaultAsync(c => c.Id == dto.CaregiverId);

        if (caregiver == null)
            throw new Exception("Caregiver not found.");

        if (!caregiver.IsApprovedByAdmin)
            throw new Exception("Caregiver is not approved.");

        if (!caregiver.IsAvaliable)
            throw new Exception("Caregiver is not available.");

        var contract = new Contract
        {
            ClientId = dto.ClientId,
            CaregiverId = dto.CaregiverId,
            JobRequestId = dto.JobRequestId
        };

        var createdContract = await _repository.CreateAsync(contract);

        caregiver.IsAvaliable = false;
        await _context.SaveChangesAsync();

        return createdContract;
    }

    public async Task<List<Contract>> GetByClientIdAsync(
        Guid clientId)
    {
        var contracts = await _repository.GetAllAsync();

        return contracts
            .Where(x => x.ClientId == clientId)
            .ToList();
    }

    public async Task<Contract?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task UpdateStatusAsync(
        UpdateContractStatusDto dto)
    {
        var contract = await _repository.GetByIdAsync(
            dto.ContractId);

        if (contract == null)
            throw new Exception("Contract not found.");

        contract.Status = dto.Status;

        if (dto.Status == ContractStatus.Successful ||
            dto.Status == ContractStatus.Cancelled)
        {
            contract.EndDate = DateTime.UtcNow;
        }
        else
        {
            contract.EndDate = null;
        }

        await _repository.UpdateAsync(contract);

        var caregiver = await _context.Caregivers
            .FirstOrDefaultAsync(
                c => c.Id == contract.CaregiverId);

        if (caregiver == null)
            return;

        if (dto.Status == ContractStatus.Successful ||
            dto.Status == ContractStatus.Cancelled)
        {
            var hasOtherActiveContract =
                await _context.Contracts.AnyAsync(c =>
                    c.CaregiverId == contract.CaregiverId &&
                    c.Id != contract.Id &&
                    c.Status != ContractStatus.Successful &&
                    c.Status != ContractStatus.Cancelled);

            if (!hasOtherActiveContract)
            {
                caregiver.IsAvaliable =
                    caregiver.IsApprovedByAdmin;
            }
        }
        else
        {
            caregiver.IsAvaliable = false;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<Contract>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
}
