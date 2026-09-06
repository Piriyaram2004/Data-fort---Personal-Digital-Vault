using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public interface IDocumentService
    {
        Task<List<DocumentResponse>> GetDocumentsAsync(int userId);

        Task<DocumentResponse> CreateDocumentAsync(
            CreateDocumentRequest request,
            int userId);

        Task<DocumentResponse?> UpdateDocumentAsync(
    int documentId,
    UpdateDocumentRequest request,
    int userId);
    }
}