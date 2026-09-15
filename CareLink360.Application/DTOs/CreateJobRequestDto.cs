namespace CareLink360.Application.DTOs
{
    public class CreateJobRequestDto
    {
        public Guid ClientId { get; set; }

        public Guid CaregiverId { get; set; }

        public Guid JobTypeId { get; set; }

        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}