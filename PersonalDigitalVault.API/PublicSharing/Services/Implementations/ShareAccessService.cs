using PersonalDigitalVault.API.PublicSharing.DTOs;
using PersonalDigitalVault.API.PublicSharing.Services.Interfaces;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.PublicSharing.Services.Implementations
{
    public class ShareAccessService : IShareAccessService
    {
        private readonly IShareLinkRepository _shareLinkRepository;

        public ShareAccessService(
            IShareLinkRepository shareLinkRepository)
        {
            _shareLinkRepository = shareLinkRepository;
        }

        public async Task<PublicShareLinkDto?> GetPublicShareAsync(
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

            return new PublicShareLinkDto
            {
                FileName = shareLink.Document.OriginalFileName,
                FileType = shareLink.Document.FileType,
                FileSize = shareLink.Document.FileSize,
                ExpiresAt = shareLink.ExpiresAt
            };
        }
    }
}