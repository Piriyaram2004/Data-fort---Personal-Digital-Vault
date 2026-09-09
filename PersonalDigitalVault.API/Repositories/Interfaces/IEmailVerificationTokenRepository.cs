using PersonalDigitalVault.API.Models;

namespace PersonalDigitalVault.API.Repositories.Interfaces
{
    public interface IEmailVerificationTokenRepository
    {
        Task AddAsync(EmailVerificationToken token);

        Task<EmailVerificationToken?> GetValidTokenAsync(
            string tokenHash,
            DateTime currentUtc);

        Task SaveChangesAsync();
    }
}