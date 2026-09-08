using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface ISearchRepository
    {
        Task<List<Folder>> SearchFoldersAsync(
            int userId,
            string searchTerm);

        Task<List<Document>> SearchDocumentsAsync(
            int userId,
            string searchTerm);

        Task<List<Credential>> SearchCredentialsAsync(
            int userId,
            string searchTerm);
    }
}