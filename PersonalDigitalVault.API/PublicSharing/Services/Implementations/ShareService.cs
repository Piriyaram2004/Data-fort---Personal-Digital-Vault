using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.PublicSharing.DTOs;
using PersonalDigitalVault.API.PublicSharing.Services.Interfaces;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.PublicSharing.Services.Implementations
{
    public class ShareService : IShareService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IShareLinkRepository _shareLinkRepository;
        private readonly ITokenService _tokenService;

        public ShareService(
            IDocumentRepository documentRepository,
            IShareLinkRepository shareLinkRepository,
            ITokenService tokenService)
        {
            _documentRepository = documentRepository;
            _shareLinkRepository = shareLinkRepository;
            _tokenService = tokenService;
        }

        public async Task<ShareLinkDto?> CreateShareLinkAsync(
            int userId,
            CreateShareLinkDto request)
        {
            var document = await _documentRepository
                .GetByIdAsync(request.DocumentId);

            if (document == null ||
                document.IsDeleted ||
                document.UserId != userId)
            {
                return null;
            }

            var shareToken = _tokenService.GenerateToken();

            var shareLink = new ShareLink
            {
                DocumentId = document.DocumentId,
                UserId = userId,
                ShareToken = shareToken,
                ExpiresAt = request.ExpiresAt,
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow
            };

            await _shareLinkRepository.AddAsync(shareLink);

            return new ShareLinkDto
            {
                ShareLinkId = shareLink.ShareLinkId,
                DocumentId = shareLink.DocumentId,
                ShareToken = shareLink.ShareToken,
                ExpiresAt = shareLink.ExpiresAt,
                IsRevoked = shareLink.IsRevoked,
                CreatedAt = shareLink.CreatedAt
            };
        }

        public async Task<List<ShareLinkDto>> GetShareLinksAsync(int userId)
        {
            var shareLinks = await _shareLinkRepository
                .GetByUserIdAsync(userId);

            return shareLinks.Select(shareLink => new ShareLinkDto
            {
                ShareLinkId = shareLink.ShareLinkId,
                DocumentId = shareLink.DocumentId,
                ShareToken = shareLink.ShareToken,
                ExpiresAt = shareLink.ExpiresAt,
                IsRevoked = shareLink.IsRevoked,
                CreatedAt = shareLink.CreatedAt
            }).ToList();
        }
    }
}