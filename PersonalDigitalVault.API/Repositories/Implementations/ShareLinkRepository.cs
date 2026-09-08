using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Data;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Repositories.Implementations
{
    public class ShareLinkRepository : IShareLinkRepository
    {
        private readonly ApplicationDbContext _context;

        public ShareLinkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ShareLink shareLink)
        {
            await _context.ShareLinks.AddAsync(shareLink);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ShareLink>> GetByUserIdAsync(int userId)
        {
            return await _context.ShareLinks
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<ShareLink?> GetByIdAndUserIdAsync(
            int shareLinkId,
            int userId)
        {
            return await _context.ShareLinks
                .FirstOrDefaultAsync(s =>
                    s.ShareLinkId == shareLinkId &&
                    s.UserId == userId);
        }

        public async Task UpdateAsync(ShareLink shareLink)
        {
            _context.ShareLinks.Update(shareLink);
            await _context.SaveChangesAsync();
        }

        public async Task<ShareLink?> GetByTokenAsync(string token)
        {
            return await _context.ShareLinks
                .Include(s => s.Document)
                .FirstOrDefaultAsync(s => s.ShareToken == token);
        }
    }
}