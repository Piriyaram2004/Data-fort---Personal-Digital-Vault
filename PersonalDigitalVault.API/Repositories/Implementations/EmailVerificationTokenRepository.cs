using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Data;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Repositories.Implementations
{
    public class EmailVerificationTokenRepository
        : IEmailVerificationTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public EmailVerificationTokenRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            EmailVerificationToken token)
        {
            await _context.EmailVerificationTokens
                .AddAsync(token);

            await _context.SaveChangesAsync();
        }

        public async Task<EmailVerificationToken?> GetValidTokenAsync(
            string tokenHash,
            DateTime currentUtc)
        {
            return await _context.EmailVerificationTokens
                .FirstOrDefaultAsync(t =>
                    t.TokenHash == tokenHash &&
                    !t.IsUsed &&
                    t.ExpiresAt > currentUtc);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}