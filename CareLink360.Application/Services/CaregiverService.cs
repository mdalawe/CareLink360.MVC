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

            _passwordHasher =
                new PasswordHasher<ApplicationUser>();
        }

       
        public async Task<CaregiverResponseDto> CreateAsync(
            CreateCaregiverDto dto)
        {
            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == dto.Email);

            if (emailExists)
            {
                throw new Exception(
                    "A user with this email already exists."
                );
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

            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    dto.Password
                );

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

                YearsOfExperience =
                    dto.YearsOfExperience,

                Skills = dto.Skills,

                IsAvaliable = true,

                IsNinVerified = false,

                IsBackgroundChecked = false,

                IsApprovedByAdmin = false,

                IsMedicallyCleared = false,

                AverageRating = 0,

                CreatedAt = DateTime.UtcNow
            };

            caregiver.Status =
                CalculateStatus(caregiver);



            var result =
                await _caregiverRepository
                    .CreateAsync(caregiver);

            return MapToResponse(result);
        }




        public async Task<List<CaregiverResponseDto>> GetAllAsync()
        {
            var caregivers =
                await _caregiverRepository.GetAllAsync();

            return caregivers
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<CaregiverResponseDto?> GetByIdAsync(
            Guid id)
        {
            var caregiver =
                await _caregiverRepository
                    .GetByIdAsync(id);

            if (caregiver == null)
                return null;

            return MapToResponse(caregiver);
        }



        public async Task<CaregiverResponseDto?> UpdateAsync(
            Guid id,
            UpdateCaregiverDto dto)
        {
            var caregiver =
                await _caregiverRepository
                    .GetByIdAsync(id);

            if (caregiver == null)
                return null;

            caregiver.PhoneNumber =
                dto.PhoneNumber;

            caregiver.Address =
                dto.Address;

            caregiver.City =
                dto.City;

            caregiver.YearsOfExperience =
                dto.YearsOfExperience;

            caregiver.Skills =
                dto.Skills;

            caregiver.IsAvaliable =
                dto.IsAvaliable;

            caregiver.Status =
                CalculateStatus(caregiver);

            var updated =
                await _caregiverRepository
                    .UpdateAsync(caregiver);

            if (updated == null)
                return null;

            return MapToResponse(updated);
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _caregiverRepository
                .DeleteAsync(id);
        }


        private static CaregiverStatus CalculateStatus(
            Caregiver caregiver)
        {
            if (!caregiver.IsNinVerified ||
                !caregiver.IsBackgroundChecked)
            {
                return CaregiverStatus.Pending;
            }

            if (caregiver.IsApprovedByAdmin &&
                caregiver.IsMedicallyCleared)
            {
                return CaregiverStatus.Active;
            }

            return CaregiverStatus.UnderReview;
        }



        private static CaregiverResponseDto MapToResponse(
            Caregiver caregiver)
        {
            return new CaregiverResponseDto
            {
                Id = caregiver.Id,

                FirstName =
                    caregiver.FirstName,

                LastName =
                    caregiver.LastName,

                PhoneNumber =
                    caregiver.PhoneNumber,

                City =
                    caregiver.City,

                YearsOfExperience =
                    caregiver.YearsOfExperience,

                IsAvaliable =
                    caregiver.IsAvaliable,

                IsNinVerified =
                    caregiver.IsNinVerified,

                IsApprovedByAdmin =
                    caregiver.IsApprovedByAdmin,

                AverageRating =
                    (double)caregiver.AverageRating
            };
        }

        public async Task<CaregiverResponseDto?> GetByUserIdAsync(Guid userId)
        {
            var caregiver = await _caregiverRepository.GetByUserIdAsync(userId);

            if (caregiver == null)
                return null;

            return MapToResponse(caregiver);
        }
    }
}