using System.ComponentModel.DataAnnotations;

namespace CareLink360.Application.DTOs
{
    public class UpdateClientDto
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? City { get; set; }

        public string? Address { get; set; }
    }
}
