namespace PersonalDigitalVault.API.Models
{
    public class ShareLink
    {
        public int ShareLinkId { get; set; }

        public int DocumentId { get; set; }

        public int UserId { get; set; }

        public string ShareToken { get; set; } = string.Empty;

        public DateTime? ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTime? RevokedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? CreatedIP { get; set; }

        // Navigation Properties
        public Document Document { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}