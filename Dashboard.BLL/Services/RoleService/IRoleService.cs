using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.RoleService
{
    public interface IRoleService
    {
        Task<ServiceResponse> CreateAsync(RoleVM model);
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetByNameAsync(string name);
        Task<ServiceResponse> GetByIdAsync(string id);
        Task<ServiceResponse> DeleteAsync(string id);
        Task<ServiceResponse> UpdateAsync(RoleVM model);
    }
}
