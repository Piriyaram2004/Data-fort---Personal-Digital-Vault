using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;
using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public class FolderService : IFolderService
    {
        // dependency injection of the IFolderRepository to interact with the data layer
        private readonly IFolderRepository _folderRepository;

        public FolderService(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }
        // Create a new folder for the user with the provided details
        public async Task<Folder?> CreateFolderAsync(
            CreateFolderRequest request,
            int userId)
        {
            // 1. Clean the folder name by removing leading and trailing whitespace
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
            if (request.ParentFolderId.HasValue) // Check if a parent folder ID has a value
            {
                var parentFolder = await _folderRepository.GetByIdAsync(
                    request.ParentFolderId.Value);

                if (parentFolder == null)
                {
                    throw new KeyNotFoundException(
                        "Parent folder not found.");
                }

                if (parentFolder.UserId != userId) // check if the parentfolder user with  current user id
                                                   // current user id comes from the jwt token
                                                   // and we get it from the controller
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
        // Get all active folders for a specific user and
        // return them as a list of FolderResponse DTOs
        public async Task<List<FolderResponse>> GetFoldersAsync(int userId)
        {
            var folders = await _folderRepository.GetByUserIdAsync(userId); // get all folders
                                                                            // for the user from the repository

            return folders.Select(folder => new FolderResponse // create a new FolderResponse DTO for each folder
            {
                FolderId = folder.FolderId, // create the copy of the folder properties to the response DTO
                UserId = folder.UserId,
                ParentFolderId = folder.ParentFolderId,
                FolderName = folder.FolderName,
                Description = folder.Description,
                IsDeleted = folder.IsDeleted,
                CreatedAt = folder.CreatedAt,
                UpdatedAt = folder.UpdatedAt
            }).ToList();
        }
        public async Task<Folder?> UpdateFolderAsync(
    int folderId,
    UpdateFolderRequest request,
    int userId)
        {
            // Find the existing folder
            var folder = await _folderRepository.GetByIdAsync(folderId); // find the folder by its ID
                                                                         // using the repository

            if (folder == null)
            {
                throw new KeyNotFoundException(
                    "Folder not found.");
            }

            // Check folder ownership
            if (folder.UserId != userId) // check folder.userid with the current user id from the jwt token
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this folder.");
            }

            // Clean the folder name
            var folderName = request.FolderName.Trim(); // request means the request from the client to
                                                        // update the folder name and description
                                                        // through the UpdateFolderRequest DTO

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

            return folder; // return the updated folder entity
        }


        public async Task<bool> DeleteFolderAsync(
    int folderId,
    int userId)
        {
            // 1. Find the active folder
            var folder = await _folderRepository.GetByIdAsync(folderId);

            if (folder == null)
            {
                throw new KeyNotFoundException(
                    "Folder not found.");
            }

            // 2. Check folder ownership
            if (folder.UserId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this folder.");
            }

            // 3. Soft delete the folder
            await _folderRepository.DeleteAsync(folder);

            return true;
        }
    }
}