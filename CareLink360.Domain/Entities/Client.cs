using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CareLink360.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string HomeAddress { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsNinVerified { get; set; }

        public ICollection<JobRequest> JobRequests { get; set; }
    = new List<JobRequest>();
    }
}
