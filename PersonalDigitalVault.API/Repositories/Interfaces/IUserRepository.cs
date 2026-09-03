using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);

        Task<bool> UserNameExistsAsync(string userName);

        Task<User?> GetByEmailAsync(string email);

        Task AddAsync(User user);
    }
}