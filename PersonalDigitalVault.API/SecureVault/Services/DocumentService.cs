using PersonalDigitalVault.API.Repositories.Interfaces;
using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<List<DocumentResponse>> GetDocumentsAsync(int userId)
        {
            var documents =
                await _documentRepository.GetByUserIdAsync(userId);

            return documents.Select(document => new DocumentResponse
            {
                DocumentId = document.DocumentId,
                UserId = document.UserId,
                FolderId = document.FolderId,
                OriginalFileName = document.OriginalFileName,
                FileType = document.FileType,
                FileSize = document.FileSize,
                SHA256Hash = document.SHA256Hash,
                IsEncrypted = document.IsEncrypted,
                CreatedAt = document.CreatedAt,
                UpdatedAt = document.UpdatedAt
            }).ToList();
        }
    }
}