using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Data;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Repositories.Implementations
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ApplicationDbContext _context;

        public SearchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Folder>> SearchFoldersAsync(
            int userId,
            string searchTerm)
        {
            return await _context.Folders
                .Where(f =>
                    f.UserId == userId &&
                    !f.IsDeleted &&
                    (f.FolderName.Contains(searchTerm) ||
                     (f.Description != null &&
                      f.Description.Contains(searchTerm))))
                .ToListAsync();
        }

        public async Task<List<Document>> SearchDocumentsAsync(
            int userId,
            string searchTerm)
        {
            return await _context.Documents
                .Where(d =>
                    d.UserId == userId &&
                    !d.IsDeleted &&
                    (d.OriginalFileName.Contains(searchTerm) ||
                     d.FileType.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<List<Credential>> SearchCredentialsAsync(
            int userId,
            string searchTerm)
        {
            return await _context.Credentials
                .Where(c =>
                    c.UserId == userId &&
                    (c.Title.Contains(searchTerm) ||
                     c.UserName.Contains(searchTerm) ||
                     (c.Notes != null &&
                      c.Notes.Contains(searchTerm))))
                .ToListAsync();
        }
    }
}