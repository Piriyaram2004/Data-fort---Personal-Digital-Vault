using PersonalDigitalVault.API.Authentication.DTOs;
using System.Net.Mail;

namespace PersonalDigitalVault.API.Authentication.Validators
{
    public class LoginRequestValidator
    {
        public List<string> Validate(LoginRequestDto request)
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

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                errors.Add("Password is required.");
            }

            return errors;
        }
    }
}