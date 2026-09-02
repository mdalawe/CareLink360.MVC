using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.DTOs
{
    public class CreateMedicalRecordDto
    {
        public Guid CaregiverId { get; set; }

        public string BloodGroup { get; set; } = string.Empty;

        public string VaccinationStatus { get; set; } = string.Empty;
    }
}
