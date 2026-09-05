using PersonalDigitalVault.API.PublicSharing.DTOs;

namespace PersonalDigitalVault.API.PublicSharing.Services.Interfaces
{
    public interface IShareAccessService
    {
        Task<PublicShareLinkDto?> GetPublicShareAsync(
            string token);
    }
}