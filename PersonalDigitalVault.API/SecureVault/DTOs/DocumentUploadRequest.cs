using Microsoft.AspNetCore.Http;

namespace PersonalDigitalVault.API.SecureVault.DTOs
{
    public class DocumentUploadRequest
    {
        public IFormFile File { get; set; } = null!;

        public int? FolderId { get; set; }
    }
}