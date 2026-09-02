using CareLink360.Domain.Entities;

public interface IMedicalRepository
{
    Task<MedicalRecord> CreateAsync(MedicalRecord record);

    Task<MedicalRecord?> GetByIdAsync(Guid id);

    Task<List<MedicalRecord>> GetByCaregiverIdAsync(Guid caregiverId);

    Task UpdateAsync(MedicalRecord record);

}

