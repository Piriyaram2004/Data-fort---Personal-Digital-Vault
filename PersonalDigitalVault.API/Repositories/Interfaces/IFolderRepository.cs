using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IFolderRepository
    {
        Task<Folder?> GetByIdAsync(int folderId);

        Task<bool> ExistsByNameAsync(
            int userId,
            int? parentFolderId,
            string normalizedFolderName);

        Task AddAsync(Folder folder);
    }
}