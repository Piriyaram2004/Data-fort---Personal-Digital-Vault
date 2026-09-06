using Microsoft.EntityFrameworkCore;
using PersonalDigitalVault.API.Data;
using PersonalDigitalVault.API.Models;
using PersonalDigitalVault.API.Repositories.Interfaces;

namespace PersonalDigitalVault.API.Repositories.Implementations
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Document?> GetByIdAsync(int documentId)
        {
            return await _context.Documents
                .FirstOrDefaultAsync(d =>
                    d.DocumentId == documentId &&
                    !d.IsDeleted);
        }

        public async Task<List<Document>> GetByUserIdAsync(int userId)
        {
            return await _context.Documents
                .Where(d =>
                    d.UserId == userId &&
                    !d.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(
            int userId,
            int? folderId,
            string normalizedFileName)
        {
            return await _context.Documents.AnyAsync(d =>
                d.UserId == userId &&
                d.FolderId == folderId &&
                d.NormalizedFileName == normalizedFileName &&
                !d.IsDeleted);
        }

        public async Task AddAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Document document)
        {
            _context.Documents.Update(document);
            await _context.SaveChangesAsync();
        }
    }
}