namespace PersonalDigitalVault.API.SecureVault.DTOs
{
    public class UpdateDocumentRequest
    {
        public string OriginalFileName { get; set; } = string.Empty;

        public int? FolderId { get; set; }
    }
}