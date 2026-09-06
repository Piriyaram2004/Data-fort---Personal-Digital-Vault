namespace PersonalDigitalVault.API.SecureVault.Services
{
    public interface IEncryptionService
    {
        byte[] GenerateIV();

        byte[] GetKey();

        Guid GetKeyId();
    }
}