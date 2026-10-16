namespace CareLink360.Application.DTOs
{
    public class JobRequestResponseDto
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public Guid CaregiverId { get; set; }

        public string CaregiverName { get; set; } = string.Empty;

        public Guid JobTypeId { get; set; }

        public string JobTypeName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime RequestedAt { get; set; }
    }
}

