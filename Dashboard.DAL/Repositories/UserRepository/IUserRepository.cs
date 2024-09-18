using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.DAL.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<bool> CheckEmailAsync(string email);
        Task<bool> CheckUserNameAsync(string userName);
        Task<bool> CheckPasswordAsync(UserVM model, string password);
        Task<UserVM?> GetUserByEmailAsync(string email);
        Task<UserVM?> GetUserByNameAsync(string userName);
        Task<UserVM?> GetUserByIdAsync(string id);
        Task<UserVM?> SignUpAsync(SignUpVM model);
        Task<IdentityResult> AddToRoleAsync(string id, string role);
        Task<string?> GenerateEmailConfirmationTokenAsync(UserVM model);
        Task<IdentityResult> CreateAsync(CreateUserVM model);
        Task<IdentityResult> UpdateAsync(UserVM model);
        Task<IdentityResult> DeleteAsync(UserVM model);
        Task<List<UserVM>> GetAllAsync();
    }
}
