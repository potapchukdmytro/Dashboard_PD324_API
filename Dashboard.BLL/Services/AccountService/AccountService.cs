using AutoMapper;
using Dashboard.BLL.Services.EmailService;
using Dashboard.BLL.Services.JwtService;
using Dashboard.DAL;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.Repositories.UserRepository;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Dashboard.BLL.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public AccountService(IEmailService emailService, IConfiguration configuration, IUserRepository userRepository, IMapper mapper, IJwtService jwtService)
        {
            _emailService = emailService;
            _configuration = configuration;
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<ServiceResponse> SignUpAsync(SignUpVM model)
        {
            if (await _userRepository.CheckEmailAsync(model.Email))
            {
                return ServiceResponse.GetBadRequestResponse(message: "Помилка реєстрації", errors: $"Пошта {model.Email} вже використовується");
            }

            if (await _userRepository.CheckUserNameAsync(model.UserName))
            {
                return ServiceResponse.GetBadRequestResponse(message: "Помилка реєстрації", errors: $"Ім'я користувача {model.UserName} вже використовується");
            }

            var newUser = _mapper.Map<User>(model);

            var user = await _userRepository.SignUpAsync(newUser, model.Password);

            var userVM = _mapper.Map<UserVM>(user);

            if (user == null)
            {
                return ServiceResponse.GetBadRequestResponse(message: "Помилка реєстрації", errors: "Не вдалося зареєструвати користувача");
            }

            var token = await _userRepository.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendConfirmitaionEmailMessageAsync(userVM, token);

            await _userRepository.AddToRoleAsync(user.Id.ToString(), Settings.UserRole);

            var tokens = await _jwtService.GenerateTokensAsync(user);

            return ServiceResponse.GetOkResponse("Успішна реєстрація", tokens.Payload);
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

                var user = await _userRepository.GetUserByEmailAsync(model.Email, true);

                var passwordResult = await _userRepository.CheckPasswordAsync(user, model.Password);

                if (!passwordResult)
                {
                    return ServiceResponse.GetBadRequestResponse(message: "Не успішний вхід", errors: "Пошта або пароль вказані невірно");
                }

                if (!user.EmailConfirmed)
                {
                    return ServiceResponse.GetBadRequestResponse(message: "Не успішний вхід", errors: "Необхідно підтвердити пошту");
                }

                var response = await _jwtService.GenerateTokensAsync(user);

                if (response.Success)
                {
                    return ServiceResponse.GetOkResponse("Успішний вхід", response.Payload);
                }
                else
                {
                    return ServiceResponse.GetBadRequestResponse("Не вдалося увійти", errors: response.Errors.ToArray());
                }
            }
            catch (Exception ex)
            {
                return ServiceResponse.GetInternalServerErrorResponse(message: "Помилка авторизації", errors: ex.Message);
            }
        }

        public async Task<ServiceResponse> EmailConfirmationAsync(string userId, string token)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return ServiceResponse.GetBadRequestResponse($"Користувача з id {userId} не знайдено", errors: $"User {user} not found");
            }

            var bytes = WebEncoders.Base64UrlDecode(token);
            var validToken = Encoding.UTF8.GetString(bytes);

            var result = await _userRepository.EmailConfirmationAsync(user, validToken);

            if (result.Succeeded)
            {
                return ServiceResponse.GetOkResponse("Пошта успішно підтверджена");
            }
            else
            {
                return ServiceResponse.GetBadRequestResponse("Не вдалося підтвердити пошту", errors: result.Errors.Select(e => e.Description).ToArray());
            }
        }

        public async Task<ServiceResponse> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return ServiceResponse.GetBadRequestResponse($"Користувача з поштою {email} не знайдено", errors: $"User {email} not found");
            }

            var token = await _userRepository.GenerateResetPasswordTokenAsync(user);

            var model = _mapper.Map<UserVM>(user);

            await _emailService.SendResetPasswordMessageAsync(model, token);

            return ServiceResponse.GetOkResponse("Лист відправлено");
        }

        public async Task<ServiceResponse> ResetPasswordAsync(ResetPasswordVM model)
        {
            var user = await _userRepository.GetUserByIdAsync(model.Id);

            if (user == null)
            {
                return ServiceResponse.GetBadRequestResponse($"Користувача з id {model.Id} не знайдено", errors: $"User {model.Id} not found");
            }

            var bytes = WebEncoders.Base64UrlDecode(model.Token);
            var validToken = Encoding.UTF8.GetString(bytes);

            var result = await _userRepository.ResetPasswordAsync(user, validToken, model.Password);

            if (!result.Succeeded)
            {
                return ServiceResponse.GetBadRequestResponse("Не вдалося скинути пароль", errors: result.Errors.Select(e => e.Description).ToArray());
            }

            return ServiceResponse.GetOkResponse("Пароль скинуто");
        }
    }
}
