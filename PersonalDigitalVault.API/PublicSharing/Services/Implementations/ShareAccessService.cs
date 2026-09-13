using PersonalDigitalVault.API.PublicSharing.DTOs;
using PersonalDigitalVault.API.PublicSharing.Services.Interfaces;
using PersonalDigitalVault.API.Repositories.Interfaces;
using PersonalDigitalVault.API.SecureVault.Services;

namespace PersonalDigitalVault.API.PublicSharing.Services.Implementations
{
    public class ShareAccessService : IShareAccessService
    {
        private readonly IShareLinkRepository _shareLinkRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IEncryptionService _encryptionService;

        public ShareAccessService(
            IShareLinkRepository shareLinkRepository,
            IFileStorageService fileStorageService,
            IEncryptionService encryptionService)
        {
            _shareLinkRepository = shareLinkRepository;
            _fileStorageService = fileStorageService;
            _encryptionService = encryptionService;
        }

        public async Task<PublicShareLinkDto?> GetPublicShareAsync(
            string token)
        {
            var shareLink = await GetValidShareLinkAsync(token);

            if (shareLink == null)
            {
                return null;
            }

            return new PublicShareLinkDto
            {
                FileName = shareLink.Document.OriginalFileName,
                FileType = shareLink.Document.FileType,
                FileSize = shareLink.Document.FileSize,
                ExpiresAt = shareLink.ExpiresAt
            };
        }

        public async Task<(byte[] FileBytes, string FileName, string ContentType)?>
            DownloadPublicShareAsync(string token)
        {
            var shareLink = await GetValidShareLinkAsync(token);

            if (shareLink == null)
            {
                return null;
            }

            var document = shareLink.Document;

            var key = _encryptionService.GetKey();

            var fileBytes = await _fileStorageService
                .ReadDecryptedFileAsync(
                    document.FilePath,
                    key,
                    document.EncryptionIV);

            return (
                fileBytes,
                document.OriginalFileName,
                document.FileType
            );
        }

        private async Task<Models.ShareLink?> GetValidShareLinkAsync(
            string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var shareLink = await _shareLinkRepository
                .GetByTokenAsync(token);

            if (shareLink == null)
            {
                return null;
            }

            if (shareLink.IsRevoked)
            {
                return null;
            }

            if (shareLink.ExpiresAt.HasValue &&
                shareLink.ExpiresAt.Value <= DateTime.UtcNow)
            {
                return null;
            }

            if (shareLink.Document == null ||
                shareLink.Document.IsDeleted)
            {
                return null;
            }

            return shareLink;
        }
    }
}