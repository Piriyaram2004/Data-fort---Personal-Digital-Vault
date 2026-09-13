namespace PersonalDigitalVault.API.PublicSharing.DTOs
{
    public class PublicShareLinkDto
    {
        public string FileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}