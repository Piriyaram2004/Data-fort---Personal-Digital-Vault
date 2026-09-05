using PersonalDigitalVault.API.Administration.DTOs;

namespace PersonalDigitalVault.API.Administration.Services
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardDto> GetDashboardAsync();
    }
}