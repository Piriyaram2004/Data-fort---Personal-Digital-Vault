using PersonalDigitalVault.API.PublicSharing.DTOs;

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

            if (request.ExpiresAt.HasValue &&
                request.ExpiresAt.Value <= DateTime.UtcNow)
            {
                return "Expiry date and time must be in the future.";
            }

            return null;
        }
    }
}