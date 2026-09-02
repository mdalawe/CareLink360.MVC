using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CareLink360.Domain.Entities
{
    public class Rating
    {
        public Guid Id { get; set; }
        public Guid CaregiverId { get; set; }
        public Guid ClientId { get; set; }

        public int Score { get; set; }
        public string Comment { get; set; }
    }
}
