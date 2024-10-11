using KoiCareSystem.Common.DTOs.Request;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace KoiCareSystem.Service.Helper
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string verifyCode, string verificationLink);

    }
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public async Task SendVerificationEmailAsync(string email, string verifyCode, string verificationLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Koicare System", "Manager"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Verify Code";

            // Email body (plain text)
            message.Body = new TextPart("plain")
            {
                Text = $@"Code: {verifyCode}
                ==========================================
                Please verify your email by clicking on this link: {verificationLink}

                If you didn't register for this account, please ignore this email.

                Thank you!"
            };
            await _smtpClient.SendAsync(message);
        }


    }
}
