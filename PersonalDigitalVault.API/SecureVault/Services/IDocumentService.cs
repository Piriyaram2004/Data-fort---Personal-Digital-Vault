using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public interface IDocumentService
    {
        Task<List<DocumentResponse>> GetDocumentsAsync(int userId);
    }
}