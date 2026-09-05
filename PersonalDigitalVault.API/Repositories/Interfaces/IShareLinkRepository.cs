using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IShareLinkRepository
    {
        Task AddAsync(ShareLink shareLink);

        Task<List<ShareLink>> GetByUserIdAsync(int userId);
    }
}