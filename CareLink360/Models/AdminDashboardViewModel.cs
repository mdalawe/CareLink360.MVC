namespace CareLink360.Web.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalCaregivers { get; set; }

        public int TotalClients { get; set; }

        public int TotalJobRequests { get; set; }

        public int TotalPayments { get; set; }

        public int TotalContracts { get; set; }

        public int TotalMedicalRecords { get; set; }

        public int TotalRatings { get; set; }

        public int PendingNinVerifications { get; set; }

        public int ActiveCaregivers { get; set; }

        public int PendingCaregivers { get; set; }

        public decimal AverageRating { get; set; }
    }
}