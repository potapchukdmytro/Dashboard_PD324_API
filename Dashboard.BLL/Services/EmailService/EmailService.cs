using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Text;

namespace Dashboard.BLL.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmailService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task SendConfirmitaionEmailMessageAsync(UserVM model, string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            var validateToken = WebEncoders.Base64UrlEncode(bytes);

            string? host = _configuration["Host:Address"];
            string confirmUrl = $"{host}Account/EmailConfirmation?u={model.Id}&t={validateToken}";
            string htmlPath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "confirmemail.html");
            string html = File.ReadAllText(htmlPath);
            html = html.Replace("confirmUrl", confirmUrl);

            string emailBody = html;
            await SendEmailAsync(model.Email, "Підтвердження", emailBody);
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

        public async Task SendResetPasswordMessageAsync(UserVM model, string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(bytes);

            string? host = _configuration["Host:Address"];
            string htmlPath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "resetpassword.html");
            string html = File.ReadAllText(htmlPath);
            html = html.Replace("userId", model.Id.ToString());
            html = html.Replace("valueToken", validToken);

            string emailBody = html;
            await SendEmailAsync(model.Email, "Скидання паролю", emailBody);
        }
    }
}
