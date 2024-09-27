using Dashboard.DAL.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.DAL.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<bool> CheckEmailAsync(string email);
        Task<bool> CheckUserNameAsync(string userName);
        Task<bool> CheckPasswordAsync(User model, string password);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByNameAsync(string userName);
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> SignUpAsync(User model, string password);
        Task<IdentityResult> AddToRoleAsync(string id, string role);
        Task<string?> GenerateEmailConfirmationTokenAsync(User model);
        Task<string> GenerateResetPasswordTokenAsync(User model);
        Task<IdentityResult> CreateAsync(User model, string password, string role);
        Task<IdentityResult> UpdateAsync(User model);
        Task<IdentityResult> DeleteAsync(User model);
        Task<List<User>> GetAllAsync();
        Task<IdentityResult> EmailConfirmationAsync(User user, string token);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
    }
}
