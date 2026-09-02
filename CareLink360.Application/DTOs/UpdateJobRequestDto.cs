using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.DTOs
{
    public class UpdateJobRequestDto
    {
        public Guid JobRequestId { get; set; }

        public string Location { get; set; }

        public string Describption { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
