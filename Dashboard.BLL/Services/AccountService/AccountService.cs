using Dashboard.BLL.Services.EmailService;
using Dashboard.DAL;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.Repositories.UserRepository;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dashboard.BLL.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserRepository _userRepository;

        public AccountService(UserManager<User> userManager, IEmailService emailService, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IUserRepository userRepository)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _userRepository = userRepository;
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

        public async Task<ServiceResponse> SignUpAsync(SignUpVM model)
        {
            if(await _userRepository.CheckEmailAsync(model.Email))
            {
                return ServiceResponse.GetBadRequestResponse(message: "Помилка реєстрації", errors: $"Пошта {model.Email} вже використовується");
            }

            if (await _userRepository.CheckUserNameAsync(model.UserName))
            {
                return ServiceResponse.GetBadRequestResponse(message: "Помилка реєстрації", errors: $"Ім'я користувача {model.UserName} вже використовується");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                NormalizedUserName = model.UserName.ToUpper()
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description);
                return ServiceResponse.GetBadRequestResponse(message: "Помилка реєстрації", errors: errors.ToArray());
            }

            await SendConfirmitaionEmailMessageAsync(user);

            await _userManager.AddToRoleAsync(user, Settings.UserRole);
            return ServiceResponse.GetOkResponse("Успішна реєстрація", "token");
        }

        public async Task<ServiceResponse> SignInAsync(SignInVM model)
        {
            try
            {
                var emailResult = await _userRepository.CheckEmailAsync(model.Email);

                if (!emailResult)
                {
                    return ServiceResponse.GetBadRequestResponse(message: "Не успішний вхід", errors: "Пошта або пароль вказані невірно");
                }

                var user = await _userManager.FindByEmailAsync(model.Email);

                var passwordResult = await _userManager.CheckPasswordAsync(user, model.Password);

                if (!passwordResult)
                {
                    return ServiceResponse.GetBadRequestResponse(message: "Не успішний вхід", errors: "Пошта або пароль вказані невірно");
                }

                var claims = new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("email", user.Email)
                };

                var issuer = _configuration["AuthSettings:issuer"];
                var audience = _configuration["AuthSettings:audience"];
                var keyString = _configuration["AuthSettings:key"];
                var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
                    );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return ServiceResponse.GetOkResponse("Успішний вхід", jwt);
            }
            catch (Exception ex)
            {
                return ServiceResponse.GetInternalServerErrorResponse(message: "Помилка авторизації", errors: ex.Message);
            }
        }
    }
}
