using System.Security.Cryptography;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public class HashService : IHashService
    {
        public async Task<string> ComputeSHA256Async(Stream stream)
        {
            using var sha256 = SHA256.Create();

            var hash = await sha256.ComputeHashAsync(stream);

            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}