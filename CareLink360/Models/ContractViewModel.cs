using System.ComponentModel.DataAnnotations;

namespace CareLink360.Web.Models
{
    public class ContractViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public Guid CaregiverId { get; set; }

        [Required]
        public Guid JobRequestId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Status { get; set; } = "Active";
    }
}