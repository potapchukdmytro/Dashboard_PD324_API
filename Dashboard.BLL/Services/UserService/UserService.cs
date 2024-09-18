using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.Repositories.UserRepository;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.BLL.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ServiceResponse> CreateAsync(CreateUserVM model)
        {
            var emailCheckResult = await _userRepository.CheckEmailAsync(model.Email);

            if(emailCheckResult)
            {
                return ServiceResponse.GetBadRequestResponse(message: "Не вдалося створити користувача", errors: $"Користувач з поштою {model.Email} вже існує");
            }

            var result = await _userRepository.CreateAsync(model);

            if (result.Succeeded)
            {
                return ServiceResponse.GetOkResponse($"Користувач {model.UserName} успішно створений");
            }

            return ServiceResponse.GetBadRequestResponse(message: "Не вдалося створити користувача", errors: result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<ServiceResponse> DeleteAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return ServiceResponse.GetBadRequestResponse(message: "Не вдалося видалити користвача", errors: $"Користувача {id} не знайдено");
            }

            var result = await _userRepository.DeleteAsync(user);

            if(!result.Succeeded)
            {
                return ServiceResponse.GetBadRequestResponse(message: "Не вдалося видалити користувача", errors: result.Errors.Select(e => e.Description).ToArray());
            }

            return ServiceResponse.GetOkResponse("Користувач успішно видалений");
        }

        public async Task<ServiceResponse> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                return ServiceResponse.GetOkResponse("Список користувачів", users);
            }
            catch (Exception ex)
            {
                return ServiceResponse.GetInternalServerErrorResponse(message: "Помилка під час отримання користувачів", errors: ex.Message);
            }
        }

        public async Task<ServiceResponse> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return ServiceResponse.GetBadRequestResponse(message: "Не вдалося отримати користвача", errors: $"Користувача {id} не знайдено");
            }

            return ServiceResponse.GetOkResponse(message: "Користувача отримано", payload: user);
        }

        public async Task<ServiceResponse> UpdateAsync(UserVM model)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(model.Id.ToString());

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

                var updateResult = await _userRepository.UpdateAsync(user);

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
