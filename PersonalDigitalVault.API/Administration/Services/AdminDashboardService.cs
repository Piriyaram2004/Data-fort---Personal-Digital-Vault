using PersonalDigitalVault.API.Administration.DTOs;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Administration.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _adminDashboardRepository;

        public AdminDashboardService(IAdminDashboardRepository adminDashboardRepository)
        {
            _adminDashboardRepository = adminDashboardRepository;
        }

        public async Task<AdminDashboardDto> GetDashboardAsync()
        {
            return new AdminDashboardDto
            {
                TotalUsers = await _adminDashboardRepository.GetTotalUsersAsync(),
                ActiveUsers = await _adminDashboardRepository.GetActiveUsersAsync(),
                TotalFolders = await _adminDashboardRepository.GetTotalFoldersAsync(),
                TotalDocuments = await _adminDashboardRepository.GetTotalDocumentsAsync(),
                TotalCredentials = await _adminDashboardRepository.GetTotalCredentialsAsync(),
                TotalShareLinks = await _adminDashboardRepository.GetTotalShareLinksAsync(),
                TotalAuditLogs = await _adminDashboardRepository.GetTotalAuditLogsAsync()
            };
        }
    }
}