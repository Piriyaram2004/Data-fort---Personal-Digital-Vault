using PersonalDigitalVault.API.DTOs.Administration;

namespace PersonalDigitalVault.API.Administration.Services
{
    public interface IAdminUserService
    {
        Task<List<AdminUserDto>> GetAllUsersAsync();
    }
}