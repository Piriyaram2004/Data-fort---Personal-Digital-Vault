using PersonalDigitalVault.API.Authentication.DTOs;
using System.Net.Mail;

namespace PersonalDigitalVault.API.Authentication.Validators
{
    public class ForgotPasswordRequestValidator
    {
        public List<string> Validate(ForgotPasswordRequestDto request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                errors.Add("Email is required.");
                return errors;
            }

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

            return errors;
        }
    }
}