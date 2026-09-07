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
        // Dependency Injection of ApplicationDbContext into the folderrepository
        // reason : to access the database and perform CRUD operations on the Folders table
        private readonly ApplicationDbContext _context;

        public FolderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add New Folder
        public async Task AddAsync(Folder folder)
        {
            await _context.Folders.AddAsync(folder); 
            await _context.SaveChangesAsync(); //save changes to the database (sql server)
        }

        // Check whether an active folder already exists
        // using UserId, ParentFolderId and NormalizedFolderName
        // it returns true if an active folder exists, otherwise false
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
        public async Task<Folder?> GetByIdAsync(int folderId) // search for a folder by its ID
                                                              // and return it if it exists and is not deleted or
                                                              // return null if it doesn't exist or is deleted
        {
            return await _context.Folders.FirstOrDefaultAsync(f =>
                f.FolderId == folderId &&
                !f.IsDeleted);
        }

        // Get all  active folders for a user
        public async Task<List<Folder>> GetByUserIdAsync(int userId)
        {
            return await _context.Folders
                .Where(f => f.UserId == userId && !f.IsDeleted) // filter method to get only active folders for the user
                .ToListAsync(); // return the list of active folders for the user
        }
        // Update an existing folder
        public async Task UpdateAsync(Folder folder)
        {
            _context.Folders.Update(folder);
            await _context.SaveChangesAsync();
        }
        // Soft delete an existing folder so we don't use the Remove
        // method to delete the folder from the database
        public async Task DeleteAsync(Folder folder)
        {
            folder.IsDeleted = true;
            folder.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}