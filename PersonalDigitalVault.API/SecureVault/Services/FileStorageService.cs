using System.Security.Cryptography;

namespace PersonalDigitalVault.API.SecureVault.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _documentsPath;

        public FileStorageService(IWebHostEnvironment environment)
        {
            _documentsPath = Path.Combine(
                environment.ContentRootPath,
                "Storage",
                "ProtectedFiles",
                "Documents");

            Directory.CreateDirectory(_documentsPath);
        }

        public async Task<string> SaveEncryptedFileAsync(
            Stream inputStream,
            string storedFileName,
            byte[] key,
            byte[] iv)
        {
            var fullPath = Path.Combine(
                _documentsPath,
                storedFileName);

            await using var fileStream =
                new FileStream(
                    fullPath,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None);

            using var aes = Aes.Create();

            aes.Key = key;
            aes.IV = iv;

            await using var cryptoStream =
                new CryptoStream(
                    fileStream,
                    aes.CreateEncryptor(),
                    CryptoStreamMode.Write);

            await inputStream.CopyToAsync(cryptoStream);

            await cryptoStream.FlushAsync();

            return fullPath;
        }

        public async Task<byte[]> ReadDecryptedFileAsync(
            string filePath,
            byte[] key,
            byte[] iv)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    "Document file was not found.");
            }

            using var aes = Aes.Create();

            aes.Key = key;
            aes.IV = iv;

            await using var fileStream =
                new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read);

            await using var cryptoStream =
                new CryptoStream(
                    fileStream,
                    aes.CreateDecryptor(),
                    CryptoStreamMode.Read);

            using var memoryStream = new MemoryStream();

            await cryptoStream.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }

        public Task DeleteFileAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }
    }
}