using System.Net.Mail;
using PersonalDigitalVault.API.Authentication.DTOs;

namespace PersonalDigitalVault.API.Authentication.Validators
{
    public class UpdateProfileRequestValidator
    {
        public List<string> Validate(UpdateProfileRequestDto request)
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
                    var address = new MailAddress(request.Email);

                    if (address.Address != request.Email.Trim())
                    {
                        errors.Add("Email format is invalid.");
                    }
                }
                catch
                {
                    errors.Add("Email format is invalid.");
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

            return errors;
        }
    }
}