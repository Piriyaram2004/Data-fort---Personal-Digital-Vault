using PersonalDigitalVault.API.Administration.DTOs;

namespace PersonalDigitalVault.API.Administration.Services
{
    public interface IAdminAuditLogService
    {
        Task<List<AdminAuditLogDto>> GetAllAsync();
    }
}