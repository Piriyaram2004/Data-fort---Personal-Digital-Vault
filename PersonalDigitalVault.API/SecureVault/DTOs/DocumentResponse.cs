namespace PersonalDigitalVault.API.SecureVault.DTOs
{
    public class DocumentResponse
    {
        public int DocumentId { get; set; }

        public int UserId { get; set; }

        public int? FolderId { get; set; }

        public string OriginalFileName { get; set; } = string.Empty;

        public string FileType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string SHA256Hash { get; set; } = string.Empty;

        public bool IsEncrypted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}