using System.ComponentModel.DataAnnotations;

namespace CareLink360.Web.Models
{
    public class PaymentViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ContractId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";
    }
}