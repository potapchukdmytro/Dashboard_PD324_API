using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.RoleService
{
    public interface IRoleService
    {
        Task<ServiceResponse> GetAllAsync();
    }
}
