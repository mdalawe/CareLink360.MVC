using CareLink360.Domain.Entities;

namespace CareLink360.Domain.Interface
{
    public interface ICaregiverRepository
    {
        Task<Caregiver> CreateAsync(Caregiver caregiver);
        Task<List<Caregiver>> GetAllAsync();
        Task<Caregiver?> GetByIdAsync(Guid id);
        Task<Caregiver?> GetByUserIdAsync(Guid userId);
        Task<Caregiver?> UpdateAsync(Caregiver caregiver);
        Task<bool> DeleteAsync(Guid id);
        Task<List<Caregiver>> GetApprovedAvailableAsync();
    }
}