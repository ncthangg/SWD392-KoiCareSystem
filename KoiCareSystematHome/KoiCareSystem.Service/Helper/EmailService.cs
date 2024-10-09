using MailKit.Net.Smtp;
using MimeKit;

namespace KoiCareSystem.Service.Helper
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string verificationLink);
    }
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task SendVerificationEmailAsync(string email, string verificationLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("tự tìm tên đi", "email-không-biết"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Verify your email";

            // Email body (plain text)
            message.Body = new TextPart("plain")
            {
                Text = $"Please verify your email by clicking on this link: {verificationLink}"
            };
            await _smtpClient.SendAsync(message);
        }
    }
}
