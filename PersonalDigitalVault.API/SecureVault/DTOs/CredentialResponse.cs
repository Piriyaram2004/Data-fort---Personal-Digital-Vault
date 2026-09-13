namespace PersonalDigitalVault.API.SecureVault.DTOs
{
    public class CredentialResponse
    {
        public int CredentialId { get; set; }

        public int UserId { get; set; }

        public int? FolderId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}