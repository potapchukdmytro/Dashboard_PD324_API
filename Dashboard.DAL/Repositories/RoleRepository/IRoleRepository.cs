using Dashboard.DAL.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.DAL.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<bool> IsUniqueNameAsync(string name);
        Task<Role?> GetByNameAsync(string name);
        Task<Role?> GetByIdAsync(Guid id);
        Task<IdentityResult> CreateAsync(Role model);
        Task<IdentityResult> UpdateAsync(Role model);
        Task<IdentityResult> DeleteAsync(Role model);
    }
}
