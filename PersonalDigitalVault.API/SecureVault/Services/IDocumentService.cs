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

        Task<bool> DeleteDocumentAsync(
    int documentId,
    int userId);

        Task<DocumentResponse> UploadDocumentAsync(
    DocumentUploadRequest request,
    int userId);

        Task<(byte[] FileBytes, string FileName, string ContentType)>
    DownloadDocumentAsync(
        int documentId,
        int userId);

        Task<bool> VerifyDocumentIntegrityAsync(
    int documentId,
    int userId);
    }
}