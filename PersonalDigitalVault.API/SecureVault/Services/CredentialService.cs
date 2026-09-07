using PersonalDigitalVault.API.Repositories.Interfaces;
using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public class CredentialService : ICredentialService
    {
        private readonly ICredentialRepository _credentialRepository;
        private readonly IEncryptionService _encryptionService;

        public CredentialService(
            ICredentialRepository credentialRepository,
            IEncryptionService encryptionService)
        {
            _credentialRepository = credentialRepository;
            _encryptionService = encryptionService;
        }

        public async Task<List<CredentialResponse>> GetCredentialsAsync(
            int userId)
        {
            var credentials =
                await _credentialRepository.GetByUserIdAsync(userId);

            return credentials.Select(credential =>
                new CredentialResponse
                {
                    CredentialId = credential.CredentialId,
                    UserId = credential.UserId,
                    FolderId = credential.FolderId,
                    Title = credential.Title,
                    UserName = credential.UserName,
                    Password = _encryptionService.DecryptCredential(
                        credential.PasswordEncrypted),
                    Notes = credential.Notes,
                    CreatedAt = credential.CreatedAt,
                    UpdatedAt = credential.UpdatedAt
                }).ToList();
        }
    }
}