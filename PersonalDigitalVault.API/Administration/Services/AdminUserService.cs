using PersonalDigitalVault.API.DTOs.Administration;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Administration.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminUserRepository _adminUserRepository;

        public AdminUserService(IAdminUserRepository adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;
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
    }
}