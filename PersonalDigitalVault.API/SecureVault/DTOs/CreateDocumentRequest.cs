namespace PersonalDigitalVault.API.SecureVault.DTOs
{
    public class CreateDocumentRequest
    {
        public string OriginalFileName { get; set; } = string.Empty;

        public int? FolderId { get; set; }

        public string FileType { get; set; } = string.Empty;

        public long FileSize { get; set; }
    }
}