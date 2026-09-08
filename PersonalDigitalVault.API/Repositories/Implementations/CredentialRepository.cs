using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Data;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Repositories.Implementations
{
    public class CredentialRepository : ICredentialRepository
    {
        private readonly ApplicationDbContext _context;

        public CredentialRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Credential>> GetByUserIdAsync(int userId)
        {
            return await _context.Credentials
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Credential?> GetByIdAsync(
            int credentialId,
            int userId)
        {
            return await _context.Credentials
                .FirstOrDefaultAsync(c =>
                    c.CredentialId == credentialId &&
                    c.UserId == userId);
        }

        public async Task AddAsync(Credential credential)
        {
            await _context.Credentials.AddAsync(credential);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Credential credential)
        {
            _context.Credentials.Update(credential);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Credential credential)
        {
            _context.Credentials.Remove(credential);
            await _context.SaveChangesAsync();
        }
    }
}