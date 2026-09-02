using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CareLink360.Domain.Entities
{
    public class MedicalRecord
    {
        public Guid Id { get; set; }
        public Guid CaregiverId { get; set; }

        public string BloodGroup { get; set; }
        public string VaccinationStatus { get; set; }
        public string ClearanceStatus { get; set; }
        public DateTime LastCheckupDate { get; set; }
    }
}