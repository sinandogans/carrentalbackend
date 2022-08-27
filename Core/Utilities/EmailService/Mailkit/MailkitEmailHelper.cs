using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Core.Utilities.EmailService.Mailkit
{
    public class MailkitEmailHelper : IEmailHelper
    {
        private readonly EmailSettings _emailSettings;

        public MailkitEmailHelper(IConfiguration configuration)
        {
            _emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
        }

        public void SendEmail(EmailMessage message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(System.Text.Encoding.UTF8, _emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(new MailboxAddress(System.Text.Encoding.UTF8, message.ReceiverName, message.ReceiverEmail));
            email.Subject = message.Subject;
            email.Body = new TextPart()
            {
                Text = message.Body
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_emailSettings.Smtp);
                client.Authenticate(_emailSettings.Username, _emailSettings.Password);
                client.Send(email);
                client.Disconnect(true);
            }

        }
    }
}
