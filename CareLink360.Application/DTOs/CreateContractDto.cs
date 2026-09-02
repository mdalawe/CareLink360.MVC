using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.DTOs
{
    public class CreateContractDto
    {
        public Guid ClientId { get; set; }

        public Guid CaregiverId { get; set; }

        public Guid JobRequestId { get; set; }
    }
}
