
using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Context;
using CareLink360.Domain.Entities;
using CareLink360.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace CareLink360.Application.Services
{
    public class JobRequestService : IJobRequestService
    {
        private readonly IJobRequestRepository _repository;
        private readonly IContractService _contractService;
        private readonly ApplicationDbContext _context;

        public JobRequestService(
            IJobRequestRepository repository,
            IContractService contractService,
            ApplicationDbContext context)
        {
            _repository = repository;
            _contractService = contractService;
            _context = context;
        }

        public async Task<JobRequest> CreateAsync(
            CreateJobRequestDto dto)
        {
            var caregiver = await _context.Caregivers
                .FirstOrDefaultAsync(c => c.Id == dto.CaregiverId);

            if (caregiver == null ||
                !caregiver.IsApprovedByAdmin ||
                !caregiver.IsAvaliable)
            {
                throw new Exception(
                    "This caregiver is not currently available for requests.");
            }

            var request = new JobRequest
            {
                ClientId = dto.ClientId,
                CaregiverId = dto.CaregiverId,
                JobTypeId = dto.JobTypeId,
                Location = dto.Location,
                Description = dto.Description
            };

            return await _repository.CreateAsync(request);
        }

        public async Task<JobRequest?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<JobRequest>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AcceptAsync(Guid id)
        {
            var request = await _repository.GetByIdAsync(id);

            if (request == null)
                throw new Exception("Job request not found.");

            var caregiver = await _context.Caregivers
                .FirstOrDefaultAsync(c => c.Id == request.CaregiverId);

            if (caregiver == null)
                throw new Exception("Caregiver not found.");

            if (!caregiver.IsApprovedByAdmin)
                throw new Exception("This caregiver has not been approved.");

            if (!caregiver.IsAvaliable)
                throw new Exception(
                    "This caregiver is no longer available.");

            request.Status = "Accepted";

            await _repository.UpdateAsync(request);

            await _contractService.CreateAsync(new CreateContractDto
            {
                ClientId = request.ClientId,
                CaregiverId = request.CaregiverId,
                JobRequestId = request.Id
            });

            caregiver.IsAvaliable = false;

            await _context.SaveChangesAsync();
        }

        public async Task RejectAsync(Guid id)
        {
            var request = await _repository.GetByIdAsync(id);

            if (request == null)
                throw new Exception("Job request not found.");

            request.Status = "Rejected";

            await _repository.UpdateAsync(request);
        }
    }
}
