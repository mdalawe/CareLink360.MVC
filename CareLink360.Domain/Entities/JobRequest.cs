namespace CareLink360.Domain.Entities
{
    public class JobRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }

        public Guid CaregiverId { get; set; }

        public Guid JobTypeId { get; set; }

        public string Status { get; set; } = "Pending";
        // Pending, Accepted, Rejected, Cancelled

        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    }
}