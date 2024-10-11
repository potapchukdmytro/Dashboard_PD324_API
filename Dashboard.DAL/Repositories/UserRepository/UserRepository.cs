using Dashboard.DAL.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Principal;

namespace Dashboard.DAL.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddToRoleAsync(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User {id} not found" });
            }

            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<bool> CheckPasswordAsync(User model, string password)
        {
            if (model == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(model, password);
        }

        public async Task<bool> CheckUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }

        public async Task<IdentityResult> CreateAsync(User model, string password, string role)
        {
            var createResult = await _userManager.CreateAsync(model, password);

            if(createResult.Succeeded)
            {
                return await _userManager.AddToRoleAsync(model, role);
            }

            return createResult;
        }

        public async Task<IdentityResult> DeleteAsync(User model)
        {
            if (model == null)
            {
                return null;
            }

            return await _userManager.DeleteAsync(model);
        }

        public async Task<IdentityResult> EmailConfirmationAsync(User user, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result;
        }

        public async Task<string?> GenerateEmailConfirmationTokenAsync(User model)
        {
            if (model == null)
            {
                return null;
            }

            return await _userManager.GenerateEmailConfirmationTokenAsync(model);
        }

        public async Task<string> GenerateResetPasswordTokenAsync(User model)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(model);
            return token;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .AsNoTracking();

            return await users.ToListAsync();
        }

        public async Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate, bool includes = false)
        {
            User? user = null;

            if (includes)
            {
                user = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(predicate);
            }
            else
            {
                user = await _userManager.Users
                .FirstOrDefaultAsync(predicate);
            }

            return user;
        }

        public async Task<User?> GetUserByEmailAsync(string email, bool includes = false)
        {
            return await GetUserAsync(u => u.NormalizedEmail == email.ToUpper(), includes);
        }

        public async Task<User?> GetUserByIdAsync(string id, bool includes = false)
        {
            return await GetUserAsync(u => u.Id.ToString() == id, includes);
        }

        public async Task<User?> GetUserByNameAsync(string userName, bool includes = false)
        {
            return await GetUserAsync(u => u.NormalizedUserName == userName.ToUpper(), includes);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result;
        }

        public async Task<IdentityResult> SetRoleAsync(User user, string role)
        {
            if (await _userManager.IsInRoleAsync(user, role))
            {
                return IdentityResult.Success;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            var result = await _userManager.AddToRoleAsync(user, role);
            return result;
        }

        public async Task<User?> SignUpAsync(User model, string password)
        {
            var result = await _userManager.CreateAsync(model, password);

            if (!result.Succeeded)
            {
                return null;
            }

            return model;
        }

        public async Task<IdentityResult> UpdateAsync(User model)
        {
            if (model == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Користувача не знайдено" });
            }

            return await _userManager.UpdateAsync(model);
        }
    }
}
