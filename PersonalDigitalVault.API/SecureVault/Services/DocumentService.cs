using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;
using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IFolderRepository _folderRepository;

        public DocumentService(
            IDocumentRepository documentRepository,
            IFolderRepository folderRepository)
        {
            _documentRepository = documentRepository;
            _folderRepository = folderRepository;
        }

        public async Task<List<DocumentResponse>> GetDocumentsAsync(
            int userId)
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

        public async Task<DocumentResponse> CreateDocumentAsync(
            CreateDocumentRequest request,
            int userId)
        {
            var originalFileName =
                request.OriginalFileName.Trim();

            if (string.IsNullOrWhiteSpace(originalFileName))
            {
                throw new ArgumentException(
                    "File name is required.");
            }

            if (request.FileSize < 0)
            {
                throw new ArgumentException(
                    "File size cannot be negative.");
            }

            if (request.FolderId.HasValue)
            {
                var folder = await _folderRepository.GetByIdAsync(
                    request.FolderId.Value);

                if (folder == null)
                {
                    throw new KeyNotFoundException(
                        "Folder not found.");
                }

                if (folder.UserId != userId)
                {
                    throw new UnauthorizedAccessException(
                        "You do not have access to this folder.");
                }
            }

            var normalizedFileName =
                originalFileName.ToLowerInvariant();

            var documentExists =
                await _documentRepository.ExistsByNameAsync(
                    userId,
                    request.FolderId,
                    normalizedFileName);

            if (documentExists)
            {
                throw new InvalidOperationException(
                    "A document with this name already exists.");
            }

            var document = new Document
            {
                UserId = userId,
                FolderId = request.FolderId,
                OriginalFileName = originalFileName,
                NormalizedFileName = normalizedFileName,
                FileType = request.FileType.Trim(),
                FileSize = request.FileSize,
                StoredFileName = string.Empty,
                FilePath = string.Empty,
                EncryptionIV = Array.Empty<byte>(),
                EncryptionKeyId = Guid.Empty,
                SHA256Hash = string.Empty,
                IsEncrypted = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _documentRepository.AddAsync(document);

            return new DocumentResponse
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
            };
        }
    }
}