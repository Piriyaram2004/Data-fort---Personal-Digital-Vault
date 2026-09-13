using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public interface ISearchService
    {
        Task<SearchResponse> SearchAsync(
            int userId,
            string searchTerm);
    }
}