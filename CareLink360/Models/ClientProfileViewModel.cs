using System.ComponentModel.DataAnnotations;

namespace CareLink360.Web.Models
{
    public class ClientProfileViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? PhoneNumber { get; set; }

        public string? HomeAddress { get; set; }

        public string? City { get; set; }
    }
}
