using Dashboard.DAL.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Dashboard.DAL.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<bool> CheckEmailAsync(string email);
        Task<bool> CheckUserNameAsync(string userName);
        Task<bool> CheckPasswordAsync(User model, string password);
        Task<User?> GetUserByEmailAsync(string email, bool includes = false);
        Task<User?> GetUserByNameAsync(string userName, bool includes = false);
        Task<User?> GetUserByIdAsync(string id, bool includes = false);
        Task<User?> SignUpAsync(User model, string password);
        Task<IdentityResult> AddToRoleAsync(string id, string role);
        Task<string?> GenerateEmailConfirmationTokenAsync(User model);
        Task<string> GenerateResetPasswordTokenAsync(User model);
        Task<IdentityResult> CreateAsync(User model, string password, string role);
        Task<IdentityResult> UpdateAsync(User model);
        Task<IdentityResult> DeleteAsync(User model);
        Task<List<User>> GetAllAsync();
        Task<User?> GetUserAsync(Expression<Func<User, bool>> predicate, bool includes = false);
        Task<IdentityResult> EmailConfirmationAsync(User user, string token);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
        Task<IdentityResult> SetRoleAsync(User user, string role);
    }
}
