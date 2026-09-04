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
            
        }
        //Checking Folder Exist Using Id , ParentFolderId , NormalizedFolderName Findout Duplicates
        public  async Task<bool> ExistsByNameAsync(int userId, int? parentFolderId, string normalizedFolderName)
        {
            return await _context.Folders.AnyAsync(f =>
                f.UserId == userId &&
                f.ParentFolderId == parentFolderId &&
                f.NormalizedFolderName == normalizedFolderName &&
                !f.IsDeleted);
        }
        // Get or Find the Folder ...
        public  async Task<Folder?> GetByIdAsync(int folderId)
        {
            return await _context.Folders.FirstOrDefaultAsync(f => f.FolderId == folderId);
        }
    }
}
