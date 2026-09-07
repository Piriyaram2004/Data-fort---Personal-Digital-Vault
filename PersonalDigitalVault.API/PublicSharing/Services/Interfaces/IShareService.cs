using PersonalDigitalVault.API.PublicSharing.DTOs;

namespace PersonalDigitalVault.API.PublicSharing.Services.Interfaces
{
    public interface IShareService
    {
        Task<ShareLinkDto?> CreateShareLinkAsync(
            int userId,
            CreateShareLinkDto request);

        Task<List<ShareLinkDto>> GetShareLinksAsync(
            int userId);

        Task<ShareLinkDto?> UpdateShareLinkAsync(
            int userId,
            int shareLinkId,
            DateTime? expiresAt);

        Task<ShareLinkDto?> RevokeShareLinkAsync(
            int userId,
            int shareLinkId);
    }
}