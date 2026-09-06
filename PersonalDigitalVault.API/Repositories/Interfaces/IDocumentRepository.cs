using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document?> GetByIdAsync(int documentId);

        Task<List<Document>> GetByUserIdAsync(int userId);

        Task<bool> ExistsByNameAsync(
            int userId,
            int? folderId,
            string normalizedFileName);

        Task AddAsync(Document document);
    }
}