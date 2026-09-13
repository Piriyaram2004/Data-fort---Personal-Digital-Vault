using PersonalDigitalVault.API.Authentication.DTOs;

namespace PersonalDigitalVault.API.Authentication.Validators
{
    public class ChangePasswordRequestValidator
    {
        public List<string> Validate(ChangePasswordRequestDto request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
            {
                errors.Add("Current password is required.");
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                errors.Add("New password is required.");
            }
            else
            {
                if (request.NewPassword.Length < 8)
                {
                    errors.Add("New password must be at least 8 characters.");
                }

                if (!request.NewPassword.Any(char.IsUpper))
                {
                    errors.Add("New password must contain at least one uppercase letter.");
                }

                if (!request.NewPassword.Any(char.IsLower))
                {
                    errors.Add("New password must contain at least one lowercase letter.");
                }

                if (!request.NewPassword.Any(char.IsDigit))
                {
                    errors.Add("New password must contain at least one number.");
                }

                if (!request.NewPassword.Any(
                    character => !char.IsLetterOrDigit(character)))
                {
                    errors.Add("New password must contain at least one special character.");
                }
            }

            if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                errors.Add("Confirm password is required.");
            }
            else if (request.NewPassword != request.ConfirmPassword)
            {
                errors.Add("New password and confirm password do not match.");
            }

            return errors;
        }
    }
}