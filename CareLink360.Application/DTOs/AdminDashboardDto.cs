using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareLink360.Application.DTOs
{
    public class AdminDashboardDto
    {
        public int TotalCaregivers { get; set; }

        public int TotalClients { get; set; }

        public int PendingNinVerifications { get; set; }

        public int ActiveContracts { get; set; }

        public int CompletedPayments { get; set; }

        public decimal AverageCaregiverRating { get; set; }
    }
}
