using System.Net.Mail;
using System.Net;

namespace RingoMedia.Services
{
    public class EmailSender
    {
        private readonly SmtpClient _smtpClient;

        public EmailSender(string smtpHost, int smtpPort, string smtpUser, string smtpPass)
        {
            _smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string to, string subject, string? body)
        {
            var mailMessage = new MailMessage("no-reply@myproject.com", to, subject, body);
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
