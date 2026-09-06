using System.Security.Cryptography;
using System.Text;

namespace PersonalDigitalVault.API.Authentication.Helpers
{
    public class PasswordResetTokenHelper
    {
        public string GenerateToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(32);

            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public string HashToken(string token)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var hashBytes = SHA256.HashData(tokenBytes);

            return Convert.ToHexString(hashBytes);
        }
    }
}