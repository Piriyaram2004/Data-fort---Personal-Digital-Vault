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
    }
}