using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Dashboard.BLL.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string emailTo, string subject, string body)
        {
            try
            {
                string? emailFrom = _configuration["EmailService:Email"];
                string? password = _configuration["EmailService:Password"];
                string? smtpServer = _configuration["EmailService:SMTP"];
                int port = int.Parse(_configuration["EmailService:Port"]);

                var message = new MimeMessage();
                message.To.Add(InternetAddress.Parse(emailTo));
                message.From.Add(InternetAddress.Parse(emailFrom));
                message.Subject = subject;

                //html body
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;
                message.Body = bodyBuilder.ToMessageBody();                

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpServer, port, true);
                    client.Authenticate(emailFrom, password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
