using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Domain.Entities;

public class JobRequestService(
    IJobRequestRepository repository,
    IContractService contractService
) : IJobRequestService
{
    public async Task<JobRequest> CreateAsync(CreateJobRequestDto dto)
    {
        var request = new JobRequest
        {
            ClientId = dto.ClientId,
            CaregiverId = dto.CaregiverId,
            JobTypeId = dto.JobTypeId,
            Location = dto.Location,
            Description = dto.Description
        };

        return await repository.CreateAsync(request);
    }


    public async Task<JobRequest?> GetByIdAsync(Guid id)
    {
        return await repository.GetByIdAsync(id);
    }


    public async Task<List<JobRequest>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }


    public async Task AcceptAsync(Guid id)
    {
        var request = await repository.GetByIdAsync(id);

        if (request == null)
            throw new Exception("Job request not found");

        request.Status = "Accepted";

        await repository.UpdateAsync(request);

        // Auto-create the Contract now that the caregiver accepted
        await contractService.CreateAsync(new CreateContractDto
        {
            ClientId = request.ClientId,
            CaregiverId = request.CaregiverId,
            JobRequestId = request.Id
        });
    }


    public async Task RejectAsync(Guid id)
    {
        var request = await repository.GetByIdAsync(id);

        if (request == null)
            throw new Exception("Job request not found");

        request.Status = "Rejected";

        await repository.UpdateAsync(request);
    }
}