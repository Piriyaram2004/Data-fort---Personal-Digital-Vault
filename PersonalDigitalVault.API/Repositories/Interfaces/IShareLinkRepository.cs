using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IShareLinkRepository
    {
        Task AddAsync(ShareLink shareLink);

        Task<List<ShareLink>> GetByUserIdAsync(int userId);

        Task<ShareLink?> GetByIdAndUserIdAsync(
            int shareLinkId,
            int userId);

        Task UpdateAsync(ShareLink shareLink);

        Task<ShareLink?> GetByTokenAsync(string token);
    }
}