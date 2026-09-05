using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Data;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Repositories.Implementations
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public PasswordResetTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PasswordResetToken token)
        {
            await _context.PasswordResetTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResetToken?> GetValidTokenAsync(
            string tokenHash,
            DateTime currentUtc)
        {
            return await _context.PasswordResetTokens
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