using System.Net;
using System.Net.Mail;

namespace PersonalDigitalVault.API.Authentication.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendPasswordResetEmailAsync(
            string email,
            string resetLink)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPortText = _configuration["Smtp:Port"];
            var smtpUser = _configuration["Smtp:User"];
            var smtpPassword = _configuration["Smtp:Password"];

            if (string.IsNullOrWhiteSpace(smtpHost) ||
                string.IsNullOrWhiteSpace(smtpPortText) ||
                string.IsNullOrWhiteSpace(smtpUser) ||
                string.IsNullOrWhiteSpace(smtpPassword) ||
                !int.TryParse(smtpPortText, out var smtpPort))
            {
                throw new InvalidOperationException(
                    "SMTP configuration is missing.");
            }

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    smtpUser,
                    smtpPassword)
            };

            using var message = new MailMessage
            {
                From = new MailAddress(smtpUser),
                Subject = "Password Reset",
                Body =
                    $"Use the following link to reset your password:\n\n{resetLink}",
                IsBodyHtml = false
            };

            message.To.Add(email);

            try
            {
                await client.SendMailAsync(message);
            }
            catch (SmtpException)
            {
                throw new InvalidOperationException(
                    "Email could not be sent.");
            }
        }
    }
}