using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.Repositories.UserRepository;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.BLL.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<User> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse> GetAllUsersAsync()
        {
            try
            { 
                var users = await _userManager.Users.ToListAsync();
                return ServiceResponse.GetOkResponse("Список користувачів", users);
            }
            catch (Exception ex)
            {
                return ServiceResponse.GetInternalServerErrorResponse(message: "Помилка під час отримання користувачів", errors: ex.Message);
            }
        }

        public async Task<ServiceResponse> UpdateAsync(UserVM model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());

                if (user == null)
                {
                    return ServiceResponse.GetBadRequestResponse(message: "Помилка оновлення", errors: $"Користувача з id {model.Id} не знайдено");
                }

                if (user.Email != model.Email)
                {
                    if (await _userRepository.CheckEmailAsync(model.Email))
                    {
                        return ServiceResponse.GetBadRequestResponse(message: "Помилка оновлення", errors: $"Пошта {model.Email} вже використовується");
                    }

                    user.Email = model.Email;
                }

                if (user.UserName != model.UserName)
                {
                    if (await _userRepository.CheckUserNameAsync(model.UserName))
                    {
                        return ServiceResponse.GetBadRequestResponse(message: "Помилка оновлення", errors: $"Ім'я користувача {model.UserName} вже використовується");
                    }

                    user.UserName = model.UserName;
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    return ServiceResponse.GetOkResponse("Користувача успішно оновлено");
                }
                else
                {
                    return ServiceResponse.GetBadRequestResponse(message: "Помилка оновлення", errors: updateResult.Errors.Select(e => e.Description).ToArray());
                }
            }
            catch (Exception ex)
            {
                return ServiceResponse.GetInternalServerErrorResponse(message: ex.Message, errors: ex.Message);
            }
        }
    }
}
