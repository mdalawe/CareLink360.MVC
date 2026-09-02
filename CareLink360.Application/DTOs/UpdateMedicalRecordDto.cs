using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.DTOs
{
    public class UpdateMedicalClearanceDto
    {
        public Guid MedicalRecordId { get; set; }

        public string ClearanceStatus { get; set; } = string.Empty;
    }
}
