using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IAdminUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetByIdAsync(int userId);
        Task SaveChangesAsync();
    }
}