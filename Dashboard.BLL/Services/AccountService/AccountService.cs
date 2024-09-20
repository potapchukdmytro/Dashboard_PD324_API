﻿using AutoMapper;
using Dashboard.BLL.Services.EmailService;
using Dashboard.DAL;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.Repositories.UserRepository;
using Dashboard.DAL.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dashboard.BLL.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AccountService(IEmailService emailService, IConfiguration configuration, IUserRepository userRepository, IMapper mapper)
        {
            _emailService = emailService;
            _configuration = configuration;
            _userRepository = userRepository;
            _mapper = mapper;
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

                var user = await _userRepository.GetUserByEmailAsync(model.Email);

                var passwordResult = await _userRepository.CheckPasswordAsync(user, model.Password);

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
