using PersonalDigitalVault.API.DTOs.Administration;

namespace PersonalDigitalVault.API.Administration.Services
{
    public interface IAdminUserService
    {
        Task<List<AdminUserDto>> GetAllUsersAsync();

        Task<bool> UpdateUserStatusAsync(
            int userId,
            bool isActive,
            int adminUserId,
            string? ipAddress);
    }
}