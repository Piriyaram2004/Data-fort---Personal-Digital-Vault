using System.Security.Cryptography;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IConfiguration _configuration;

        public EncryptionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public byte[] GenerateIV()
        {
            return RandomNumberGenerator.GetBytes(16);
        }

        public byte[] GetKey()
        {
            var key = _configuration["DocumentEncryption:Key"];

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException(
                    "Document encryption key is not configured.");
            }

            return Convert.FromBase64String(key);
        }

        public Guid GetKeyId()
        {
            var keyId = _configuration["DocumentEncryption:KeyId"];

            if (!Guid.TryParse(keyId, out var result))
            {
                throw new InvalidOperationException(
                    "Document encryption key ID is not configured.");
            }

            return result;
        }
    }
}