
using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Domain.Entities;

public class JobRequestService(IJobRequestRepository repository) : IJobRequestService
    {
    public async Task<JobRequest> CreateAsync(CreateJobRequestDto dto)
        {
            var request = new JobRequest
            {
                ClientId = dto.ClientId,
                CaregiverId = dto.CaregiverId,
                JobTypeId = dto.JobTypeId
            };

            return await repository.CreateAsync(request);
        }

        public async Task UpdateStatusAsync(UpdateJobRequestDto dto)
        {
            var request = await repository.GetByIdAsync(dto.JobRequestId);

            if (request == null)
                throw new Exception("Job request not found");

            request.Status = dto.Status;

            await repository.UpdateAsync(request);
        }

        public async Task<List<JobRequest>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }
    }

