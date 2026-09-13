using PersonalDigitalVault.API.DTOs.Administration;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Administration.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IAdminAuditLogRepository _adminAuditLogRepository;

        public AdminUserService(
            IAdminUserRepository adminUserRepository,
            IAdminAuditLogRepository adminAuditLogRepository)
        {
            _adminUserRepository = adminUserRepository;
            _adminAuditLogRepository = adminAuditLogRepository;
        }

        public async Task<List<AdminUserDto>> GetAllUsersAsync()
        {
            var users = await _adminUserRepository.GetAllUsersAsync();

            return users.Select(user => new AdminUserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
                ProfileImageUrl = user.ProfileImageUrl,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                RoleName = user.Role.RoleName
            }).ToList();
        }

        public async Task<bool> UpdateUserStatusAsync(
            int userId,
            bool isActive,
            int adminUserId,
            string? ipAddress)
        {
            var user = await _adminUserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.IsActive = isActive;
            user.UpdatedAt = DateTime.UtcNow;

            var auditLog = new AuditLog
            {
                UserId = adminUserId,
                Action = "UserStatusChanged",
                EntityType = "User",
                EntityId = userId,
                Details = $"Account status changed to {(isActive ? "Active" : "Inactive")}",
                IPAddress = ipAddress,
                CreatedAt = DateTime.UtcNow
            };

            await _adminAuditLogRepository.AddAsync(auditLog);

            await _adminUserRepository.SaveChangesAsync();

            return true;
        }
    }
}