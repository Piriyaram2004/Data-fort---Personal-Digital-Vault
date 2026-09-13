using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string roleName);
    }
}