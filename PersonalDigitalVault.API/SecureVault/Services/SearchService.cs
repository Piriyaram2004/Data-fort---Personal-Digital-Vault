using PersonalDigitalVault.API.Repositories.Interfaces;
using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepository;

        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public async Task<SearchResponse> SearchAsync(
            int userId,
            string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("Search term is required.");

            searchTerm = searchTerm.Trim();

            var folders = await _searchRepository.SearchFoldersAsync(
                userId,
                searchTerm);

            var documents = await _searchRepository.SearchDocumentsAsync(
                userId,
                searchTerm);

            var credentials = await _searchRepository.SearchCredentialsAsync(
                userId,
                searchTerm);

            return new SearchResponse
            {
                Folders = folders.Select(f => new SearchFolderResponse
                {
                    FolderId = f.FolderId,
                    FolderName = f.FolderName,
                    Description = f.Description
                }).ToList(),

                Documents = documents.Select(d => new SearchDocumentResponse
                {
                    DocumentId = d.DocumentId,
                    FolderId = d.FolderId,
                    OriginalFileName = d.OriginalFileName,
                    FileType = d.FileType
                }).ToList(),

                Credentials = credentials.Select(c =>
                    new SearchCredentialResponse
                    {
                        CredentialId = c.CredentialId,
                        FolderId = c.FolderId,
                        Title = c.Title,
                        UserName = c.UserName,
                        Notes = c.Notes
                    }).ToList()
            };
        }
    }
}