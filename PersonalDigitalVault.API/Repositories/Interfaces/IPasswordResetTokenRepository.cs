using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token);

        Task<PasswordResetToken?> GetValidTokenAsync(
            string tokenHash,
            DateTime currentUtc);

        Task SaveChangesAsync();
    }
}