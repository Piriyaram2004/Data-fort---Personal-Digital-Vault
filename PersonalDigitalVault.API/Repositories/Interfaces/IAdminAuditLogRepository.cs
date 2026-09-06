using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IAdminAuditLogRepository
    {
        Task<List<AuditLog>> GetAllAsync();
    }
}