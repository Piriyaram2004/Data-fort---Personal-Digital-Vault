using PersonalDigitalVault.API.PublicSharing.DTOs;
using PersonalDigitalVault.API.PublicSharing.Helpers;

namespace PersonalDigitalVault.API.PublicSharing.Validators
{
    public class ShareLinkValidator
    {
        public string? ValidateCreate(CreateShareLinkDto request)
        {
            if (request.DocumentId <= 0)
            {
                return "A valid document ID is required.";
            }

            if (request.ExpiresAt.HasValue)
            {
                var expiresAtUtc =
                    ShareLinkTimeHelper.ConvertLocalToUtc(
                        request.ExpiresAt.Value);

                if (expiresAtUtc <= DateTime.UtcNow)
                {
                    return "Expiry date and time must be in the future.";
                }
            }

            return null;
        }
    }
}