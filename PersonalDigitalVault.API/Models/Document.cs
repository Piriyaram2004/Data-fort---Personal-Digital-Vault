namespace PersonalDigitalVault.API.Models
{
    public class Document
    {
        public int DocumentId { get; set; }

        public int UserId { get; set; }

        public int? FolderId { get; set; }

        public string OriginalFileName { get; set; } = string.Empty;

        public string NormalizedFileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string FileType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public byte[] EncryptionIV { get; set; } = Array.Empty<byte>();

        public Guid EncryptionKeyId { get; set; }

        public string SHA256Hash { get; set; } = string.Empty;

        public bool IsEncrypted { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User User { get; set; } = null!;

        public Folder? Folder { get; set; }

        public ICollection<ShareLink> ShareLinks { get; set; }
            = new List<ShareLink>();
    }
}