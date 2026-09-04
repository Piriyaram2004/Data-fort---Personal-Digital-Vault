using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.SecureVault.DTOs;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public interface IFolderService
    {
        // async method to create a new folder for a user with a dto request and userId from the authentication context.
        Task<Folder?> CreateFolderAsync(CreateFolderRequest request,int userId);

    }
}
