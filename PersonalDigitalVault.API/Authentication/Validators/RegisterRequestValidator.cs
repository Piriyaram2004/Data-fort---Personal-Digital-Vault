using System.Net.Mail;
using PersonalDigitalVault.API.Authentication.DTOs;

namespace PersonalDigitalVault.API.Authentication.Validators
{
    public class RegisterRequestValidator
    {
        public List<string> Validate(RegisterRequestDto request)
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
                    var emailAddress = new MailAddress(request.Email);

                    if (emailAddress.Address != request.Email)
                    {
                        errors.Add("Invalid email format.");
                    }
                }
                catch
                {
                    errors.Add("Invalid email format.");
                }
            }

            if (string.IsNullOrWhiteSpace(request.UserName))
            {
                errors.Add("User name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                errors.Add("Full name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                errors.Add("Password is required.");
            }
            else if (request.Password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters.");
            }

            if (request.Password != request.ConfirmPassword)
            {
                errors.Add("Password and confirm password do not match.");
            }

            return errors;
        }
    }
}