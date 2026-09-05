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
    }
}