using System.ComponentModel.DataAnnotations;

namespace CareLink360.Web.Models
{
    public class CaregiverViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public int YearsOfExperience { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsMedicallyCleared { get; set; }

        public decimal AverageRating { get; set; }
    }
}
