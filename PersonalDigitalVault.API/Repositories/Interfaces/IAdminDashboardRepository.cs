namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IAdminDashboardRepository
    {
        Task<int> GetTotalUsersAsync();
        Task<int> GetActiveUsersAsync();
        Task<int> GetTotalFoldersAsync();
        Task<int> GetTotalDocumentsAsync();
        Task<int> GetTotalCredentialsAsync();
        Task<int> GetTotalShareLinksAsync();
        Task<int> GetTotalAuditLogsAsync();
    }
}