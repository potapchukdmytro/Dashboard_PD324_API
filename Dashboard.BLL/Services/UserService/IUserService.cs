using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse> GetAllUsersAsync();
        Task<ServiceResponse> UpdateAsync(UserVM model);
    }
}
