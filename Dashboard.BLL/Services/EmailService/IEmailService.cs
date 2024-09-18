using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailTo, string subject, string body);
        Task SendConfirmitaionEmailMessageAsync(UserVM model, string token);
    }
}
