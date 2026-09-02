using CareLink360.Domain.Entities;

namespace CareLink360.Application.DTOs
{
    public class CreateJobRequestDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }

        public Guid CaregiverId { get; set; }

        public Guid JobTypeId { get; set; }

        public string Status { get; set; } = "Pending";
        // Pending, Accepted, Rejected, Cancelled

        public string Location { get; set; }

        public string Describption { get; set; 
        }
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    }
}