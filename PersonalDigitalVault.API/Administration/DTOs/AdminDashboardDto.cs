namespace PersonalDigitalVault.API.Administration.DTOs
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }

        public int ActiveUsers { get; set; }

        public int TotalFolders { get; set; }

        public int TotalDocuments { get; set; }

        public int TotalCredentials { get; set; }

        public int TotalShareLinks { get; set; }

        public int TotalAuditLogs { get; set; }
    }
}