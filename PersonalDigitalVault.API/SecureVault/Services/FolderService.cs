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

        public Task<Folder?> CreateFolderAsync(CreateFolderRequest request, int userId)
        {
            var folderName = request.FolderName.Trim(); // Trim whitespace from the folder name
            var folderNormalizedName = folderName.ToLower(); // Normalize the folder name to lowercase

            throw new NotImplementedException();


        }
    }
}