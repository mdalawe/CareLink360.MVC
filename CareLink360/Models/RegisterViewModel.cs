using System.ComponentModel.DataAnnotations;

namespace CareLink360.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }

        [Required]
        public required string Role { get; set; }
    }
}