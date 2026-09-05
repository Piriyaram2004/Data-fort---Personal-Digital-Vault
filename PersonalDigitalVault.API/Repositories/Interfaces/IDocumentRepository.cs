using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document?> GetByIdAsync(int documentId);
    }
}