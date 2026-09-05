using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Data;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Repositories.Implementations
{
    public class AdminAuditLogRepository : IAdminAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminAuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(log => log.CreatedAt)
                .ToListAsync();
        }
    }
}