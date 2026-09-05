namespace PersonalDigitalVault.API.SecureVault.DTOs
{
    public class CreateFolderRequest
    {
        public string FolderName { get; set; } = string.Empty;

        public int? ParentFolderId { get; set; }

        public string? Description { get; set; }
    }
}