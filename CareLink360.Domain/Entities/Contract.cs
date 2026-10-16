using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CareLink360.Domain.Enums; // Added namespace for the Enum

namespace CareLink360.Domain.Entities
{
    public class Contract
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }
        public Guid CaregiverId { get; set; }
        public Guid JobRequestId { get; set; }

        public Client Client { get; set; } = null!;
        public Caregiver Caregiver { get; set; } = null!;
        public JobRequest JobRequest { get; set; } = null!;

        public ContractStatus Status { get; set; } = ContractStatus.Pending;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
