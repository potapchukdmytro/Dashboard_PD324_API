using Dashboard.BLL.Services.EmailService;
using Dashboard.DAL;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Dashboard.BLL.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountService(UserManager<User> userManager, IEmailService emailService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task SendConfirmitaionEmailMessageAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var bytes = Encoding.UTF8.GetBytes(token);
            var validateToken = WebEncoders.Base64UrlEncode(bytes);

            string? host = _configuration["Host:Address"];
            string confirmUrl = $"{host}Account/EmailConfirmation?u={user.Id}&t={validateToken}";
            string htmlPath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "confirmemail.html");
            string html = File.ReadAllText(htmlPath);
            html = html.Replace("confirmUrl", confirmUrl);

            string emailBody = html;
            await _emailService.SendEmailAsync(user.Email, "Підтвердження", emailBody);
        }

        private async Task<bool> CheckEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        private async Task<bool> CheckUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }

        public async Task<ServiceResponse<string>> SignUpAsync(SignUpVM model)
        {
            if(await CheckEmailAsync(model.Email))
            {
                return ServiceResponse<string>.GetServiceResponse("Помилка реєстрації", false, null, $"Пошта {model.Email} вже використовується");
            }

            if (await CheckUserNameAsync(model.Username))
            {
                return ServiceResponse<string>.GetServiceResponse("Помилка реєстрації", false, null, $"Ім'я користувача {model.Username} вже використовується");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
                NormalizedUserName = model.Username.ToUpper()
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description);
                return ServiceResponse<string>.GetServiceResponse("Помилка реєстрації", false, null, errors.ToArray());
            }

            await SendConfirmitaionEmailMessageAsync(user);

            await _userManager.AddToRoleAsync(user, Settings.UserRole);
            return ServiceResponse<string>.GetServiceResponse("Успішна реєстрація", true, "token");
        }
    }
}
