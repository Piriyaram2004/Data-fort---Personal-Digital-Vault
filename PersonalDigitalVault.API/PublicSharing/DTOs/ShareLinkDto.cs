namespace PersonalDigitalVault.API.PublicSharing.DTOs
{
    public class ShareLinkDto
    {
        public int ShareLinkId { get; set; }

        public int DocumentId { get; set; }

        public string ShareToken { get; set; } = string.Empty;

        public DateTime? ExpiresAt { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}