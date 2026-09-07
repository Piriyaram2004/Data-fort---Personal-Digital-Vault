using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface ICredentialRepository
    {
        Task<List<Credential>> GetByUserIdAsync(int userId);
        Task<Credential?> GetByIdAsync(int credentialId, int userId);
        Task AddAsync(Credential credential);
        Task UpdateAsync(Credential credential);
        Task DeleteAsync(Credential credential);
    }
}