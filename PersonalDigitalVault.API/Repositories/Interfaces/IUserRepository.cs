using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);

        Task<bool> UserNameExistsAsync(string userName);

        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int userId);

        Task AddAsync(User user);
        Task SaveChangesAsync();

        Task<bool> EmailExistsForOtherUserAsync(string email,int userId);

        Task<bool> UserNameExistsForOtherUserAsync(string userName,int userId);
    }
}