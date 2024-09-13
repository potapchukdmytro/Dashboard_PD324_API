namespace Dashboard.BLL.Services.EmailService
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string emailTo, string subject, string body);
    }
}
