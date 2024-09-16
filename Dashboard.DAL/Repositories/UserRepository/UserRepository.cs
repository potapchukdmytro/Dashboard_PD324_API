
using Dashboard.DAL.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.DAL.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<bool> CheckUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }
    }
}
