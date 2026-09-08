namespace PersonalDigitalVault.API.SecureVault.DTOs
{
    public class FolderResponse
    {
        public int FolderId { get; set; }

        public int UserId { get; set; }

        public int? ParentFolderId { get; set; }

        public string FolderName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}