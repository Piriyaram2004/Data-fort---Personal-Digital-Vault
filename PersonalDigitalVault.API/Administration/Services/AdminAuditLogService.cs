using PersonalDigitalVault.API.Administration.DTOs;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Administration.Services
{
    public class AdminAuditLogService : IAdminAuditLogService
    {
        private readonly IAdminAuditLogRepository _adminAuditLogRepository;

        public AdminAuditLogService(
            IAdminAuditLogRepository adminAuditLogRepository)
        {
            _adminAuditLogRepository = adminAuditLogRepository;
        }

        public async Task<List<AdminAuditLogDto>> GetAllAsync()
        {
            var auditLogs = await _adminAuditLogRepository.GetAllAsync();

            return auditLogs.Select(log => new AdminAuditLogDto
            {
                AuditLogId = log.AuditLogId,
                UserId = log.UserId,
                Action = log.Action,
                EntityType = log.EntityType,
                EntityId = log.EntityId,
                Details = log.Details,
                IPAddress = log.IPAddress,
                CreatedAt = log.CreatedAt
            }).ToList();
        }
    }
}