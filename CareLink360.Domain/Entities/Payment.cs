using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public Guid ClientId { get; set; }
        public string PaymentMethod {  get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}