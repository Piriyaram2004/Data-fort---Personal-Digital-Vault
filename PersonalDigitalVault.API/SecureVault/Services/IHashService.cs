namespace PersonalDigitalVault.API.SecureVault.Services
{
    public interface IHashService
    {
        Task<string> ComputeSHA256Async(Stream stream);
    }
}