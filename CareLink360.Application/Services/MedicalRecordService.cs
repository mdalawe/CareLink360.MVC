using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Domain.Entities;

public class MedicalService(IMedicalRepository repository) : IMedicalService
    {
        public async Task<MedicalRecord> CreateAsync(CreateMedicalRecordDto dto)
        {
            var record = new MedicalRecord
            {
                CaregiverId = dto.CaregiverId,
                BloodGroup = dto.BloodGroup,
                VaccinationStatus = dto.VaccinationStatus
            };

            return await repository.CreateAsync(record);
        }

        public async Task UpdateClearanceAsync(UpdateMedicalClearanceDto dto)
        {
            var record = await repository.GetByIdAsync(dto.MedicalRecordId);

            if (record == null)
                throw new Exception("Medical record not found");

            record.ClearanceStatus = dto.ClearanceStatus;
            record.LastCheckupDate = DateTime.UtcNow;

            await repository.UpdateAsync(record);
        }

        public async Task<List<MedicalRecord>> GetByCaregiverIdAsync(Guid caregiverId)
        {
            return await repository.GetByCaregiverIdAsync(caregiverId);
        }
    }
