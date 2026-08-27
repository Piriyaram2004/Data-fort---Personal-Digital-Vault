namespace PersonalDigitalVault.API.Models
{
    public class Folder
    {
        public int FolderId { get; set; }

        public int UserId { get; set; }

        public int? ParentFolderId { get; set; }

        public string FolderName { get; set; } = string.Empty;

        public string NormalizedFolderName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User User { get; set; } = null!;

        public Folder? ParentFolder { get; set; }

        public ICollection<Folder> SubFolders { get; set; }
            = new List<Folder>();

        public ICollection<Document> Documents { get; set; }
            = new List<Document>();

        public ICollection<Credential> Credentials { get; set; }
            = new List<Credential>();
    }
}