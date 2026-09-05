using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IShareLinkRepository
    {
        Task AddAsync(ShareLink shareLink);
    }
}