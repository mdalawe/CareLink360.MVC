using CareLink360.Application.DTOs;
using CareLink360.Domain.Entities;
using CareLink360.Application.Interfaces;
using CareLik360.Domain.Interface;
using CareLink360.Domain.Interface;

namespace CareLink360.Application.Services
{
    public class NinVerificationService : INinVerificationService
    {
        private readonly INinVerificationRepository _repository;
        private readonly IClientRepository _clientRepository;

        public NinVerificationService(
            INinVerificationRepository repository,
            IClientRepository clientRepository)
        {
            _repository = repository;
            _clientRepository = clientRepository;
        }

        public async Task<object> SubmitAsync(SubmitNinDto dto)
        {
            var verification = new NinVerification
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Nin = dto.Nin,
                IsVerified = false,
                Status = "Pending",
                SubmittedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(verification);

            return new
            {
                Message = "NIN submitted successfully",
                VerificationId = verification.Id
            };
        }

        public async Task ReviewAsync(ReviewNinDto dto)
        {
            var verification = await _repository.GetByIdAsync(dto.VerificationId);

            if (verification == null)
                throw new Exception("Verification not found");

            verification.IsVerified = dto.Approve;
            verification.Status = dto.Approve ? "Approved" : "Rejected";
            verification.VerifiedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(verification);

            if (dto.Approve)
            {
                var client = await _clientRepository.GetByIdAsync(verification.UserId);

                if (client != null)
                {
                    client.IsNinVerified = true;
                    await _clientRepository.UpdateAsync(client);
                }
            }
        }
    }
}