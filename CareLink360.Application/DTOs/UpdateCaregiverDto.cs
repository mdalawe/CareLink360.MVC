namespace CareLink360.Application.DTOs
{
    public class UpdateCaregiverDto
    {
        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }

        public string Skills { get; set; } = string.Empty;

        public bool IsAvaliable { get; set; }
    }
}