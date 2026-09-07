namespace PersonalDigitalVault.API.SecureVault.DTOs
{
    public class UpdateCredentialRequest
    {
        public int? FolderId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}