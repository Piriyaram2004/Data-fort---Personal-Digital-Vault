using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;
using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public class CredentialService : ICredentialService
    {
        private readonly ICredentialRepository _credentialRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IFolderRepository _folderRepository;

        public CredentialService(
            ICredentialRepository credentialRepository,
            IEncryptionService encryptionService,
            IFolderRepository folderRepository)
        {
            _credentialRepository = credentialRepository;
            _encryptionService = encryptionService;
            _folderRepository = folderRepository;
        }

        public async Task<List<CredentialResponse>> GetCredentialsAsync(int userId)
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

        public async Task<CredentialResponse> CreateCredentialAsync(
            int userId,
            CreateCredentialRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required.");

            if (string.IsNullOrWhiteSpace(request.UserName))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.");

            if (request.FolderId.HasValue)
            {
                var folder = await _folderRepository.GetByIdAsync(
                    request.FolderId.Value);

                if (folder == null ||
                    folder.UserId != userId ||
                    folder.IsDeleted)
                {
                    throw new UnauthorizedAccessException(
                        "Invalid folder.");
                }
            }

            var credential = new Credential
            {
                UserId = userId,
                FolderId = request.FolderId,
                Title = request.Title.Trim(),
                UserName = request.UserName.Trim(),
                PasswordEncrypted =
                    _encryptionService.EncryptCredential(request.Password),
                Notes = string.IsNullOrWhiteSpace(request.Notes)
                    ? null
                    : request.Notes.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _credentialRepository.AddAsync(credential);

            return new CredentialResponse
            {
                CredentialId = credential.CredentialId,
                UserId = credential.UserId,
                FolderId = credential.FolderId,
                Title = credential.Title,
                UserName = credential.UserName,
                Password = request.Password,
                Notes = credential.Notes,
                CreatedAt = credential.CreatedAt,
                UpdatedAt = credential.UpdatedAt
            };
        }
        public async Task<CredentialResponse?> UpdateCredentialAsync(
    int credentialId,
    int userId,
    UpdateCredentialRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required.");

            if (string.IsNullOrWhiteSpace(request.UserName))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.");

            var credential = await _credentialRepository.GetByIdAsync(
                credentialId,
                userId);

            if (credential == null)
                return null;

            if (request.FolderId.HasValue)
            {
                var folder = await _folderRepository.GetByIdAsync(
                    request.FolderId.Value);

                if (folder == null ||
                    folder.UserId != userId ||
                    folder.IsDeleted)
                {
                    throw new UnauthorizedAccessException(
                        "Invalid folder.");
                }
            }

            credential.FolderId = request.FolderId;
            credential.Title = request.Title.Trim();
            credential.UserName = request.UserName.Trim();
            credential.PasswordEncrypted =
                _encryptionService.EncryptCredential(request.Password);
            credential.Notes = string.IsNullOrWhiteSpace(request.Notes)
                ? null
                : request.Notes.Trim();
            credential.UpdatedAt = DateTime.UtcNow;

            await _credentialRepository.UpdateAsync(credential);

            return new CredentialResponse
            {
                CredentialId = credential.CredentialId,
                UserId = credential.UserId,
                FolderId = credential.FolderId,
                Title = credential.Title,
                UserName = credential.UserName,
                Password = request.Password,
                Notes = credential.Notes,
                CreatedAt = credential.CreatedAt,
                UpdatedAt = credential.UpdatedAt
            };
        }
        public async Task<bool> DeleteCredentialAsync(
    int credentialId,
    int userId)
        {
            var credential = await _credentialRepository.GetByIdAsync(
                credentialId,
                userId);

            if (credential == null)
                return false;

            await _credentialRepository.DeleteAsync(credential);

            return true;
        }
    }
}