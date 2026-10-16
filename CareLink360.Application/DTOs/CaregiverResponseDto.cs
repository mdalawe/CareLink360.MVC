using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.DTOs
{
    public class CaregiverResponseDto
    {
            public Guid Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string PhoneNumber { get; set; }

            public string Address { get; set; }

            public string Skills { get; set; }

            public string City { get; set; }

            public int YearsOfExperience { get; set; }

            public bool IsAvaliable { get; set; }

            public bool IsApprovedByAdmin { get; set; }

            public double AverageRating { get; set; }
    }
}
