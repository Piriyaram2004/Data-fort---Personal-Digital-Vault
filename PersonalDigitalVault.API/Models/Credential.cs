namespace PersonalDigitalVault.API.Models
{
    public class Credential
    {
        public int CredentialId { get; set; }

        public int UserId { get; set; }

        public int? FolderId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public byte[] PasswordEncrypted { get; set; } = Array.Empty<byte>();

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User User { get; set; } = null!;

        public Folder? Folder { get; set; }
    }
}