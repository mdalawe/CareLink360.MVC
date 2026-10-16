using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Context;
using CareLink360.Domain.Entities;
using CareLink360.Domain.Enums;
using CareLink360.Domain.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareLink360.Application.Services
{
    public class CaregiverService : ICaregiverService
    {
        private readonly ICaregiverRepository _caregiverRepository;
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<ApplicationUser> _passwordHasher;

        public CaregiverService(
            ICaregiverRepository caregiverRepository,
            ApplicationDbContext context)
        {
            _caregiverRepository = caregiverRepository;
            _context = context;
            _passwordHasher = new PasswordHasher<ApplicationUser>();
        }

        // CREATE CAREGIVER
        public async Task<CaregiverResponseDto> CreateAsync(
            CreateCaregiverDto dto)
        {
            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == dto.Email);

            if (emailExists)
            {
                throw new Exception(
                    "A user with this email already exists.");
            }

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = "Caregiver",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var caregiver = new Caregiver
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Address = dto.Address,
                City = dto.City,
                YearsOfExperience = dto.YearsOfExperience,
                Skills = dto.Skills,

                // New caregivers await admin approval.
                IsAvaliable = false,
                IsApprovedByAdmin = false,

                AverageRating = 0,
                CreatedAt = DateTime.UtcNow
            };

            caregiver.Status = CalculateStatus(caregiver);

            var result = await _caregiverRepository.CreateAsync(caregiver);

            return MapToResponse(result);
        }

        // GET ALL CAREGIVERS
        public async Task<List<CaregiverResponseDto>> GetAllAsync()
        {
            var caregivers = await _caregiverRepository.GetAllAsync();

            return caregivers.Select(MapToResponse).ToList();
        }

        // GET APPROVED AND AVAILABLE CAREGIVERS
        public async Task<List<CaregiverResponseDto>>
            GetApprovedAvailableAsync()
        {
            var caregivers =
                await _caregiverRepository.GetApprovedAvailableAsync();

            return caregivers.Select(MapToResponse).ToList();
        }

        // GET CAREGIVER BY ID
        public async Task<CaregiverResponseDto?> GetByIdAsync(Guid id)
        {
            var caregiver = await _caregiverRepository.GetByIdAsync(id);

            return caregiver == null ? null : MapToResponse(caregiver);
        }

        // GET CAREGIVER BY USER ID
        public async Task<CaregiverResponseDto?> GetByUserIdAsync(
            Guid userId)
        {
            var caregiver =
                await _caregiverRepository.GetByUserIdAsync(userId);

            return caregiver == null ? null : MapToResponse(caregiver);
        }

        // UPDATE CAREGIVER PROFILE
        public async Task<CaregiverResponseDto?> UpdateAsync(
            Guid id,
            UpdateCaregiverDto dto)
        {
            var caregiver = await _caregiverRepository.GetByIdAsync(id);

            if (caregiver == null)
                return null;

            caregiver.PhoneNumber = dto.PhoneNumber;
            caregiver.Address = dto.Address;
            caregiver.City = dto.City;
            caregiver.YearsOfExperience = dto.YearsOfExperience;
            caregiver.Skills = dto.Skills;

            // Do not allow an unapproved caregiver to become available.
            if (dto.IsAvaliable && !caregiver.IsApprovedByAdmin)
            {
                throw new Exception(
                    "Only approved caregivers can be made available.");
            }

            if (dto.IsAvaliable)
            {
                var hasActiveContract = await _context.Contracts
                    .AnyAsync(c =>
                        c.CaregiverId == id &&
                        c.Status != ContractStatus.Successful &&
                        c.Status != ContractStatus.Cancelled);

                if (hasActiveContract)
                {
                    throw new Exception(
                        "This caregiver has an active contract and cannot be made available.");
                }
            }

            caregiver.IsAvaliable = dto.IsAvaliable;
            caregiver.Status = CalculateStatus(caregiver);

            var updated = await _caregiverRepository.UpdateAsync(caregiver);

            return updated == null ? null : MapToResponse(updated);
        }

        // DELETE CAREGIVER
        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _caregiverRepository.DeleteAsync(id);
        }

        // ADMIN APPROVAL STATUS
        public async Task<bool> SetApprovalStatusAsync(
            Guid id,
            bool isApproved)
        {
            var caregiver = await _caregiverRepository.GetByIdAsync(id);

            if (caregiver == null)
                return false;

            caregiver.IsApprovedByAdmin = isApproved;

            // An unapproved caregiver must never be available.
            if (!isApproved)
            {
                caregiver.IsAvaliable = false;
            }

            caregiver.Status = CalculateStatus(caregiver);

            var updated = await _caregiverRepository.UpdateAsync(caregiver);

            return updated != null;
        }

        // ADMIN AVAILABILITY STATUS
        public async Task<bool> SetAvailabilityAsync(
            Guid caregiverId,
            bool isAvailable)
        {
            var caregiver =
                await _caregiverRepository.GetByIdAsync(caregiverId);

            if (caregiver == null)
                return false;

            if (isAvailable)
            {
                if (!caregiver.IsApprovedByAdmin)
                {
                    throw new Exception(
                        "Only approved caregivers can be made available.");
                }

                var hasActiveContract = await _context.Contracts
                    .AnyAsync(c =>
                        c.CaregiverId == caregiverId &&
                        c.Status != ContractStatus.Successful &&
                        c.Status != ContractStatus.Cancelled);

                if (hasActiveContract)
                {
                    throw new Exception(
                        "This caregiver has an active contract and cannot be made available.");
                }
            }

            caregiver.IsAvaliable = isAvailable;
            caregiver.Status = CalculateStatus(caregiver);

            var updated = await _caregiverRepository.UpdateAsync(caregiver);

            return updated != null;
        }

        // CREATE CAREGIVER PROFILE
        public async Task<CaregiverResponseDto> CreateProfileAsync(
            CreateCaregiverProfileDto dto)
        {
            var existingCaregiver =
                await _caregiverRepository.GetByUserIdAsync(dto.UserId);

            if (existingCaregiver != null)
            {
                throw new Exception(
                    "A caregiver profile already exists for this user.");
            }

            var caregiver = new Caregiver
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Address = dto.Address,
                City = dto.City,
                YearsOfExperience = dto.YearsOfExperience,
                Skills = dto.Skills,

                IsAvaliable = false,
                IsApprovedByAdmin = false,

                AverageRating = 0,
                CreatedAt = DateTime.UtcNow
            };

            caregiver.Status = CalculateStatus(caregiver);

            var result = await _caregiverRepository.CreateAsync(caregiver);

            return MapToResponse(result);
        }

        // TOGGLE AVAILABILITY
        public async Task<bool> ToggleAvailabilityAsync(Guid userId)
        {
            var caregiver =
                await _caregiverRepository.GetByUserIdAsync(userId);

            if (caregiver == null)
                return false;

            // Do not allow an unapproved caregiver to become available.
            if (!caregiver.IsAvaliable && !caregiver.IsApprovedByAdmin)
            {
                throw new Exception(
                    "Your account must be approved before you can become available.");
            }

            var newAvailability = !caregiver.IsAvaliable;

            if (newAvailability)
            {
                var hasActiveContract = await _context.Contracts
                    .AnyAsync(c =>
                        c.CaregiverId == caregiver.Id &&
                        c.Status != ContractStatus.Successful &&
                        c.Status != ContractStatus.Cancelled);

                if (hasActiveContract)
                {
                    throw new Exception(
                        "You cannot become available while you have an active contract.");
                }
            }

            caregiver.IsAvaliable = newAvailability;
            caregiver.Status = CalculateStatus(caregiver);

            var updated = await _caregiverRepository.UpdateAsync(caregiver);

            return updated != null;
        }

        // DETERMINE CAREGIVER STATUS
        private static CaregiverStatus CalculateStatus(Caregiver caregiver)
        {
            return caregiver.IsApprovedByAdmin
                ? CaregiverStatus.Active
                : CaregiverStatus.Pending;
        }

        // MAP ENTITY TO RESPONSE DTO
        private static CaregiverResponseDto MapToResponse(
            Caregiver caregiver)
        {
            return new CaregiverResponseDto
            {
                Id = caregiver.Id,
                FirstName = caregiver.FirstName,
                LastName = caregiver.LastName,
                PhoneNumber = caregiver.PhoneNumber,
                City = caregiver.City,
                YearsOfExperience = caregiver.YearsOfExperience,
                IsAvaliable = caregiver.IsAvaliable,
                IsApprovedByAdmin = caregiver.IsApprovedByAdmin,
                AverageRating = (double)caregiver.AverageRating
            };
        }
    }
}