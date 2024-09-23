using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse> GetAllUsersAsync();
        Task<ServiceResponse> UpdateAsync(UserVM model);
        Task<ServiceResponse> CreateAsync(CreateUserVM model);
        Task<ServiceResponse> DeleteAsync(string id);
        Task<ServiceResponse> GetByIdAsync(string id);
        Task<ServiceResponse> GetByEmailAsync(string email);
        Task<ServiceResponse> GetByUserNameAsync(string userName);
        Task<ServiceResponse> AddImageFromUserAsync(UserImageVM model);
    }
}
