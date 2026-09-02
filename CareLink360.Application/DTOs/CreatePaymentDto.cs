using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.DTOs
{
    public class CreatePaymentDto
    {
        public Guid ContractId { get; set; }

        public Guid ClientId { get; set; }

        public string PaymentMethod { get; set; }

        public decimal Amount { get; set; }
    }
}
