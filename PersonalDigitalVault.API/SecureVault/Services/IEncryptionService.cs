namespace PersonalDigitalVault.API.SecureVault.Services
{
    public interface IEncryptionService
    {
        byte[] GenerateIV();

        byte[] GetKey();

        Guid GetKeyId();

        byte[] EncryptCredential(string value);

        string DecryptCredential(byte[] encryptedValue);
    }
}