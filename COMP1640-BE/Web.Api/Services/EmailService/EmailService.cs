using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Web.Api.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "no.reply.goldenideas@gmail.com";
        private readonly string _smtpPassword = "fhqyrmsxqjavpaay";
        private readonly bool _useSsl = false;

        private readonly string _fromName = "Golden Ideas";
        private readonly string _fromEmail = "no.reply.goldenideas@gmail.com";

        public EmailService() { }

        public async Task<bool> SendEmailAsync(string toName, string toEmail, string subject, string body)
        {
            MimeMessage message = CreateEmail(toName, toEmail, subject, body);
            return await SendEmail(message);
        }

        private async Task<bool> SendEmail(MimeMessage message)
        {
            try
            {
                var smtpClient = new SmtpClient();
                await smtpClient.ConnectAsync(_smtpServer, _smtpPort, _useSsl);
                await smtpClient.AuthenticateAsync(_smtpUsername, _smtpPassword);
                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);
                return true;
            }
            catch (Exception)
            {
                // Log the exception or do something else
                throw;
            }
        }

        private MimeMessage CreateEmail(string toName, string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_fromName, _fromEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };
            return message;
        }
    }
}
