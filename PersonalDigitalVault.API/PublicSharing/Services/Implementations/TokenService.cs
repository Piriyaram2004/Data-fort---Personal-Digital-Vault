using System.Security.Cryptography;
using PersonalDigitalVault.API.PublicSharing.Services.Interfaces;

namespace PersonalDigitalVault.API.PublicSharing.Services.Implementations
{
    public class TokenService : ITokenService
    {
        public string GenerateToken()
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(32);

            return Convert.ToBase64String(randomBytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}