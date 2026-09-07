using System.Security.Cryptography;
using System.Text;

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

        public byte[] EncryptCredential(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Credential password is required.");
            }

            var key = GetKey();
            var iv = GenerateIV();

            using var aes = Aes.Create();

            aes.Key = key;
            aes.IV = iv;

            var plainBytes =
                Encoding.UTF8.GetBytes(value);

            using var memoryStream =
                new MemoryStream();

            using (var cryptoStream = new CryptoStream(
                memoryStream,
                aes.CreateEncryptor(),
                CryptoStreamMode.Write))
            {
                cryptoStream.Write(
                    plainBytes,
                    0,
                    plainBytes.Length);

                cryptoStream.FlushFinalBlock();
            }

            var encryptedBytes =
                memoryStream.ToArray();

            var result =
                new byte[iv.Length + encryptedBytes.Length];

            Buffer.BlockCopy(
                iv,
                0,
                result,
                0,
                iv.Length);

            Buffer.BlockCopy(
                encryptedBytes,
                0,
                result,
                iv.Length,
                encryptedBytes.Length);

            return result;
        }

        public string DecryptCredential(byte[] encryptedValue)
        {
            if (encryptedValue == null ||
                encryptedValue.Length <= 16)
            {
                throw new InvalidOperationException(
                    "Encrypted credential value is invalid.");
            }

            var key = GetKey();

            var iv = new byte[16];

            Buffer.BlockCopy(
                encryptedValue,
                0,
                iv,
                0,
                iv.Length);

            var cipherBytes =
                new byte[encryptedValue.Length - iv.Length];

            Buffer.BlockCopy(
                encryptedValue,
                iv.Length,
                cipherBytes,
                0,
                cipherBytes.Length);

            using var aes = Aes.Create();

            aes.Key = key;
            aes.IV = iv;

            using var memoryStream =
                new MemoryStream(cipherBytes);

            using var cryptoStream = new CryptoStream(
                memoryStream,
                aes.CreateDecryptor(),
                CryptoStreamMode.Read);

            using var resultStream =
                new MemoryStream();

            cryptoStream.CopyTo(resultStream);

            return Encoding.UTF8.GetString(
                resultStream.ToArray());
        }
    }
}