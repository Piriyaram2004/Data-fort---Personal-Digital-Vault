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

        public async Task<Folder?> CreateFolderAsync(CreateFolderRequest request, int userId)
        {
            var folderName = request.FolderName.Trim(); // Trim whitespace from the folder name
            var folderNormalizedName = folderName.ToLower(); // Normalize the folder name to lowercase

            if (request.ParentFolderId.HasValue) // from request, parentfolderid has optional value,
                                                 // if it has value then check if the parent folder exists
                                                 // and belongs to the user
            {
                var parentFolder = await _folderRepository.GetByIdAsync(
                    request.ParentFolderId.Value);
              
                if (parentFolder == null)
                {
                    throw new KeyNotFoundException("Parent folder not found.");
                }

                if (parentFolder.UserId != userId)
                {
                    throw new UnauthorizedAccessException(
                        "You do not have access to this parent folder.");
                }

            }
            throw new NotImplementedException();


        }
    }
}