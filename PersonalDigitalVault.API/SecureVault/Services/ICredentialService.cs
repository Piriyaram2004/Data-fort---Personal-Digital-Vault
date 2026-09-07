using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public interface ICredentialService
    {
        Task<List<CredentialResponse>> GetCredentialsAsync(
            int userId);
    }
}