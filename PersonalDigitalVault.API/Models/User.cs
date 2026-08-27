namespace PersonalDigitalVault.API.Models
{
    public class User
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string? ProfileImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastPasswordChangedAt { get; set; }

        // Navigation Properties
        public Role Role { get; set; } = null!;

        public ICollection<PasswordResetToken> PasswordResetTokens { get; set; }
            = new List<PasswordResetToken>();

        public ICollection<Folder> Folders { get; set; }
            = new List<Folder>();

        public ICollection<Document> Documents { get; set; }
            = new List<Document>();

        public ICollection<Credential> Credentials { get; set; }
            = new List<Credential>();

        public ICollection<ShareLink> ShareLinks { get; set; }
            = new List<ShareLink>();

        public ICollection<AuditLog> AuditLogs { get; set; }
            = new List<AuditLog>();
    }
}