using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.DTOs
{
    public class ReviewNinDto
    {
        public Guid VerificationId { get; set; }
        public bool Approve { get; set; }
    }
}
