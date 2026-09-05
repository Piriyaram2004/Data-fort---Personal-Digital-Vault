using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Data;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Repositories.Implementations
{
    public class FolderRepository : IFolderRepository
    {
        private readonly ApplicationDbContext _context;

        public FolderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add New Folder
        public async Task AddAsync(Folder folder)
        {
            await _context.Folders.AddAsync(folder);
            await _context.SaveChangesAsync();
        }

        // Check whether an active folder already exists
        // using UserId, ParentFolderId and NormalizedFolderName
        public async Task<bool> ExistsByNameAsync(
            int userId,
            int? parentFolderId,
            string normalizedFolderName)
        {
            return await _context.Folders.AnyAsync(f =>
                f.UserId == userId &&
                f.ParentFolderId == parentFolderId &&
                f.NormalizedFolderName == normalizedFolderName &&
                !f.IsDeleted);
        }

        // Get an active folder by ID
        public async Task<Folder?> GetByIdAsync(int folderId)
        {
            return await _context.Folders.FirstOrDefaultAsync(f =>
                f.FolderId == folderId &&
                !f.IsDeleted);
        }

        // Get active folders for a user
        public async Task<List<Folder>> GetByUserIdAsync(int userId)
        {
            return await _context.Folders
                .Where(f => f.UserId == userId && !f.IsDeleted)
                .ToListAsync();
        }
        // Update an existing folder
        public async Task UpdateAsync(Folder folder)
        {
            _context.Folders.Update(folder);
            await _context.SaveChangesAsync();
        }
    }
}