using PersonalDigitalVault.API.PublicSharing.DTOs;

namespace PersonalDigitalVault.API.PublicSharing.Services.Interfaces
{
    public interface IShareAccessService
    {
        Task<PublicShareLinkDto?> GetPublicShareAsync(
            string token);

        Task<(byte[] FileBytes, string FileName, string ContentType)?>
            DownloadPublicShareAsync(string token);
    }
}