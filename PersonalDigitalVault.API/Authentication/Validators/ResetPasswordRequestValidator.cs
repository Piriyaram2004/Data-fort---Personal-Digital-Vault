using PersonalDigitalVault.API.Authentication.DTOs;
using System.Net.Mail;

namespace PersonalDigitalVault.API.Authentication.Validators
{
    public class ResetPasswordRequestValidator
    {
        public List<string> Validate(ResetPasswordRequestDto request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                errors.Add("Email is required.");
            }
            else
            {
                try
                {
                    var email = new MailAddress(request.Email.Trim());

                    if (email.Address != request.Email.Trim())
                    {
                        errors.Add("Invalid email format.");
                    }
                }
                catch
                {
                    errors.Add("Invalid email format.");
                }
            }

            if (string.IsNullOrWhiteSpace(request.Token))
            {
                errors.Add("Reset token is required.");
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