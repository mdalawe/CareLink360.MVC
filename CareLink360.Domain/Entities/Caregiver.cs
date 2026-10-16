using CareLink360.Domain.Enums;

namespace CareLink360.Domain.Entities
{
    public class Caregiver
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Skills { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }

        public bool IsAvaliable { get; set; }

        public bool IsApprovedByAdmin { get; set; }

        public decimal AverageRating { get; set; }

        public DateTime CreatedAt { get; set; }

        public CaregiverStatus Status { get; set; }

        public ICollection<JobRequest> JobRequests { get; set; }
    = new List<JobRequest>();
    }
}