using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface ICredentialRepository
    {
        Task<List<Credential>> GetByUserIdAsync(int userId);
    }
}