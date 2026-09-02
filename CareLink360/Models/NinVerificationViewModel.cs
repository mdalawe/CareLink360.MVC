using System.ComponentModel.DataAnnotations;

namespace CareLink360.Web.Models
{
    public class NinVerificationViewModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [Required(ErrorMessage = "NIN is required")]
        [StringLength(11, MinimumLength = 11,
            ErrorMessage = "NIN must be exactly 11 digits")]
        [RegularExpression(@"^\d{11}$",
            ErrorMessage = "NIN must contain only numbers")]
        public string Nin { get; set; } = string.Empty;

        public bool IsVerified { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime SubmittedAt { get; set; }
    }
}