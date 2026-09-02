using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.DTOs
{
    public class UpdatePaymentStatusDto
    {
        public Guid PaymentId { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
