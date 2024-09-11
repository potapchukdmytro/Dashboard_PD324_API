using Dashboard.DAL.Models.Identity;

namespace Dashboard.BLL.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<List<User>>> GetAllUsersAsync();
    }
}
