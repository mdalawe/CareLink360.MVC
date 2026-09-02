using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CareLink360.Domain.Entities
{
    public class TrainingProgress
    {
        public Guid Id { get; set; }
        public Guid CaregiverId { get; set; }

        public string CourseName { get; set; }
        public int ProgressPercent { get; set; }
        public bool Completed { get; set; }
    }
}