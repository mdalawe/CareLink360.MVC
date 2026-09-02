using System.ComponentModel.DataAnnotations;

namespace CareLink360.Web.Models
{
    public class JobRequestViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        public string ClientName { get; set; } = string.Empty;

        [Required]
        public Guid JobTypeId { get; set; }

        public string JobType { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime PreferredDate { get; set; }

        public string Status { get; set; } = "Pending";
    }
}