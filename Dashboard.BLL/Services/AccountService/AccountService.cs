using Dashboard.DAL;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.BLL.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;

        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
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

            await _userManager.AddToRoleAsync(user, Settings.UserRole);
            return ServiceResponse<string>.GetServiceResponse("Успішна реєстрація", true, "token");
        }
    }
}
