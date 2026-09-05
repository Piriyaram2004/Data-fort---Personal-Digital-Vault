namespace PersonalDigitalVault.API.PublicSharing.DTOs
{
    public class CreateShareLinkDto
    {
        public int DocumentId { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}