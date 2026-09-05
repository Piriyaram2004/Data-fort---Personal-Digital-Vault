using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Data;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Repositories.Implementations
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminDashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetActiveUsersAsync()
        {
            return await _context.Users.CountAsync(user => user.IsActive);
        }

        public async Task<int> GetTotalFoldersAsync()
        {
            return await _context.Folders.CountAsync();
        }

        public async Task<int> GetTotalDocumentsAsync()
        {
            return await _context.Documents.CountAsync();
        }

        public async Task<int> GetTotalCredentialsAsync()
        {
            return await _context.Credentials.CountAsync();
        }

        public async Task<int> GetTotalShareLinksAsync()
        {
            return await _context.ShareLinks.CountAsync();
        }

        public async Task<int> GetTotalAuditLogsAsync()
        {
            return await _context.AuditLogs.CountAsync();
        }
    }
}