using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;
using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;

        public FolderService(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public async Task<Folder?> CreateFolderAsync(
            CreateFolderRequest request,
            int userId)
        {
            // 1. Clean the folder name
            var folderName = request.FolderName.Trim();

            // 2. Validate that the folder name is not empty
            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new ArgumentException(
                    "Folder name is required.");
            }

            // 3. Normalize the folder name for duplicate checking
            var folderNormalizedName = folderName.ToLower();

            // 4. If a parent folder is provided,
            //    check that it exists and belongs to the current user
            if (request.ParentFolderId.HasValue)
            {
                var parentFolder = await _folderRepository.GetByIdAsync(
                    request.ParentFolderId.Value);

                if (parentFolder == null)
                {
                    throw new KeyNotFoundException(
                        "Parent folder not found.");
                }

                if (parentFolder.UserId != userId)
                {
                    throw new UnauthorizedAccessException(
                        "You do not have access to this parent folder.");
                }
            }

            // 5. Check for duplicate folder name
            var folderExists = await _folderRepository.ExistsByNameAsync(
                userId,
                request.ParentFolderId,
                folderNormalizedName);

            if (folderExists)
            {
                throw new InvalidOperationException(
                    "A folder with this name already exists.");
            }

            // 6. Create the Folder entity
            var folder = new Folder
            {
                UserId = userId,
                ParentFolderId = request.ParentFolderId,
                FolderName = folderName,
                NormalizedFolderName = folderNormalizedName,
                Description = request.Description?.Trim(),
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // 7. Save the folder through the repository
            await _folderRepository.AddAsync(folder);

            // 8. Return the newly created folder
            return folder;
        }
        public async Task<List<Folder>> GetFoldersAsync(int userId)

        {
            return await _folderRepository.GetByUserIdAsync(userId);
        }
        public async Task<Folder?> UpdateFolderAsync(
    int folderId,
    UpdateFolderRequest request,
    int userId)
        {
            // Find the existing folder
            var folder = await _folderRepository.GetByIdAsync(folderId);

            if (folder == null)
            {
                throw new KeyNotFoundException(
                    "Folder not found.");
            }

            // Check folder ownership
            if (folder.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this folder.");
            }

            // Clean the folder name
            var folderName = request.FolderName.Trim();

            // Validate folder name
            if (string.IsNullOrWhiteSpace(folderName))
            {
                throw new ArgumentException(
                    "Folder name is required.");
            }

            // Normalize the folder name
            var normalizedFolderName = folderName.ToLowerInvariant();

            // Check duplicate name if the name changed
            if (folder.NormalizedFolderName != normalizedFolderName)
            {
                var folderExists = await _folderRepository.ExistsByNameAsync(
                    userId,
                    folder.ParentFolderId,
                    normalizedFolderName);

                if (folderExists)
                {
                    throw new InvalidOperationException(
                        "A folder with this name already exists.");
                }
            }

            // Update folder
            folder.FolderName = folderName;
            folder.NormalizedFolderName = normalizedFolderName;
            folder.Description = request.Description?.Trim();
            folder.UpdatedAt = DateTime.UtcNow;

            // Save changes
            await _folderRepository.UpdateAsync(folder);

            return folder;
        }

    }
}