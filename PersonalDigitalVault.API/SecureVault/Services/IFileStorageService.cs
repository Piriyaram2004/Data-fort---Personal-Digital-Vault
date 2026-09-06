namespace PersonalDigitalVault.API.SecureVault.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveEncryptedFileAsync(
            Stream inputStream,
            string storedFileName,
            byte[] key,
            byte[] iv);

        Task<byte[]> ReadDecryptedFileAsync(
            string filePath,
            byte[] key,
            byte[] iv);

        Task DeleteFileAsync(string filePath);
    }
}