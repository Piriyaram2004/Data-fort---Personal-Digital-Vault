using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;
using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IEncryptionService _encryptionService;
        private readonly IHashService _hashService;

        public DocumentService(
            IDocumentRepository documentRepository,
            IFolderRepository folderRepository,
            IFileStorageService fileStorageService,
            IEncryptionService encryptionService,
            IHashService hashService)
        {
            _documentRepository = documentRepository;
            _folderRepository = folderRepository;
            _fileStorageService = fileStorageService;
            _encryptionService = encryptionService;
            _hashService = hashService;
        }

        // ==========================================
        // GET DOCUMENTS
        // ==========================================

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

        // ==========================================
        // CREATE DOCUMENT
        // ==========================================

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
                var folder =
                    await _folderRepository.GetByIdAsync(
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

        // ==========================================
        // UPDATE DOCUMENT
        // ==========================================

        public async Task<DocumentResponse?> UpdateDocumentAsync(
            int documentId,
            UpdateDocumentRequest request,
            int userId)
        {
            var document =
                await _documentRepository.GetByIdAsync(documentId);

            if (document == null)
            {
                throw new KeyNotFoundException(
                    "Document not found.");
            }

            if (document.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this document.");
            }

            var originalFileName =
                request.OriginalFileName.Trim();

            if (string.IsNullOrWhiteSpace(originalFileName))
            {
                throw new ArgumentException(
                    "File name is required.");
            }

            if (request.FolderId.HasValue)
            {
                var folder =
                    await _folderRepository.GetByIdAsync(
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

            if (document.NormalizedFileName != normalizedFileName ||
                document.FolderId != request.FolderId)
            {
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
            }

            document.OriginalFileName =
                originalFileName;

            document.NormalizedFileName =
                normalizedFileName;

            document.FolderId =
                request.FolderId;

            document.UpdatedAt =
                DateTime.UtcNow;

            await _documentRepository.UpdateAsync(document);

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

        // ==========================================
        // DELETE DOCUMENT
        // ==========================================

        public async Task<bool> DeleteDocumentAsync(
            int documentId,
            int userId)
        {
            var document =
                await _documentRepository.GetByIdAsync(documentId);

            if (document == null)
            {
                throw new KeyNotFoundException(
                    "Document not found.");
            }

            if (document.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this document.");
            }

            await _documentRepository.DeleteAsync(document);

            return true;
        }

        // ==========================================
        // UPLOAD DOCUMENT
        // ==========================================

        public async Task<DocumentResponse> UploadDocumentAsync(
            DocumentUploadRequest request,
            int userId)
        {
            if (request.File == null || request.File.Length == 0)
            {
                throw new ArgumentException(
                    "A file is required.");
            }

            const long maxFileSize = 10 * 1024 * 1024;

            if (request.File.Length > maxFileSize)
            {
                throw new ArgumentException(
                    "File size cannot exceed 10 MB.");
            }

            var originalFileName =
                Path.GetFileName(
                    request.File.FileName).Trim();

            if (string.IsNullOrWhiteSpace(originalFileName))
            {
                throw new ArgumentException(
                    "File name is required.");
            }

            var extension =
                Path.GetExtension(
                    originalFileName).ToLowerInvariant();

            var allowedExtensions = new[]
            {
                ".pdf",
                ".doc",
                ".docx",
                ".txt",
                ".jpg",
                ".jpeg",
                ".png"
            };

            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException(
                    "File type is not allowed.");
            }

            if (request.FolderId.HasValue)
            {
                var folder =
                    await _folderRepository.GetByIdAsync(
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

            await using var inputStream =
                request.File.OpenReadStream();

            var originalHash =
                await _hashService.ComputeSHA256Async(
                    inputStream);

            inputStream.Position = 0;

            var encryptionKey =
                _encryptionService.GetKey();

            var encryptionIV =
                _encryptionService.GenerateIV();

            var encryptionKeyId =
                _encryptionService.GetKeyId();

            var storedFileName =
                $"{Guid.NewGuid():N}.enc";

            string filePath = string.Empty;

            try
            {
                filePath =
                    await _fileStorageService.SaveEncryptedFileAsync(
                        inputStream,
                        storedFileName,
                        encryptionKey,
                        encryptionIV);

                var document = new Document
                {
                    UserId = userId,
                    FolderId = request.FolderId,
                    OriginalFileName = originalFileName,
                    NormalizedFileName = normalizedFileName,
                    StoredFileName = storedFileName,
                    FilePath = filePath,
                    FileType = request.File.ContentType ?? extension,
                    FileSize = request.File.Length,
                    EncryptionIV = encryptionIV,
                    EncryptionKeyId = encryptionKeyId,
                    SHA256Hash = originalHash,
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
            catch
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    await _fileStorageService.DeleteFileAsync(
                        filePath);
                }

                throw;
            }
        }

        // ==========================================
        // DOWNLOAD DOCUMENT
        // ==========================================

        public async Task<(
            byte[] FileBytes,
            string FileName,
            string ContentType)>
            DownloadDocumentAsync(
                int documentId,
                int userId)
        {
            var document =
                await _documentRepository.GetByIdAsync(
                    documentId);

            if (document == null)
            {
                throw new KeyNotFoundException(
                    "Document not found.");
            }

            if (document.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this document.");
            }

            if (string.IsNullOrWhiteSpace(
                document.FilePath))
            {
                throw new InvalidOperationException(
                    "Document file is not available.");
            }

            var encryptionKey =
                _encryptionService.GetKey();

            var fileBytes =
                await _fileStorageService.ReadDecryptedFileAsync(
                    document.FilePath,
                    encryptionKey,
                    document.EncryptionIV);

            var contentType =
                string.IsNullOrWhiteSpace(document.FileType)
                    ? "application/octet-stream"
                    : document.FileType;

            return (
                fileBytes,
                document.OriginalFileName,
                contentType);
        }

        // ==========================================
        // VERIFY DOCUMENT INTEGRITY
        // ==========================================

        public async Task<bool> VerifyDocumentIntegrityAsync(
            int documentId,
            int userId)
        {
            var document =
                await _documentRepository.GetByIdAsync(
                    documentId);

            if (document == null)
            {
                throw new KeyNotFoundException(
                    "Document not found.");
            }

            if (document.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this document.");
            }

            if (string.IsNullOrWhiteSpace(
                document.FilePath))
            {
                throw new InvalidOperationException(
                    "Document file is not available.");
            }

            var encryptionKey =
                _encryptionService.GetKey();

            var fileBytes =
                await _fileStorageService.ReadDecryptedFileAsync(
                    document.FilePath,
                    encryptionKey,
                    document.EncryptionIV);

            using var memoryStream =
                new MemoryStream(fileBytes);

            var currentHash =
                await _hashService.ComputeSHA256Async(
                    memoryStream);

            return string.Equals(
                currentHash,
                document.SHA256Hash,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}