using System.ComponentModel.DataAnnotations;

namespace CareLink360.Web.ViewModels.Caregiver
{
    public class CreateCaregiverProfileViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string Skills { get; set; } = string.Empty;

        [Range(0, 100)]
        public int YearsOfExperience { get; set; }
    }
}