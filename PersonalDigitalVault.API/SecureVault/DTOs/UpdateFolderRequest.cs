namespace PersonalDigitalVault.API.SecureVault.DTOs
{
    public class UpdateFolderRequest
    {
        public string FolderName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}