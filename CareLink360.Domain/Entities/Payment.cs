using System;

namespace CareLink360.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ContractId { get; set; }

        public Guid ClientId { get; set; }

        public Contract Contract { get; set; } = null!;

        public Client Client { get; set; } = null!;

        public string PaymentMethod { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}