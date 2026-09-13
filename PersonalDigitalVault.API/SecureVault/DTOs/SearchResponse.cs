namespace PersonalDigitalVault.API.SecureVault.DTOs
{
    public class SearchResponse
    {
        public List<SearchFolderResponse> Folders { get; set; } = new();
        public List<SearchDocumentResponse> Documents { get; set; } = new();
        public List<SearchCredentialResponse> Credentials { get; set; } = new();
    }

    public class SearchFolderResponse
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class SearchDocumentResponse
    {
        public int DocumentId { get; set; }
        public int? FolderId { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
    }

    public class SearchCredentialResponse
    {
        public int CredentialId { get; set; }
        public int? FolderId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}