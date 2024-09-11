using Dashboard.DAL.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.BLL.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ServiceResponse<List<User>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                return ServiceResponse<List<User>>.GetServiceResponse("Список користувачів", true, users);
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<User>>.GetServiceResponse("Помилка під час отримання користувачів", false, null, ex.Message);
            }
        }
    }
}
