using Dashboard.DAL.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DAL.Repositories.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleRepository(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateAsync(Role model)
        {
            var result = await _roleManager.CreateAsync(model);
            return result;
        }

        public async Task<IdentityResult> DeleteAsync(Role model)
        {
            var result =  await _roleManager.DeleteAsync(model);
            return result;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            var roles = _roleManager.Roles.AsNoTracking();
            return await roles.ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(Guid id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task<bool> IsUniqueNameAsync(string name)
        {
            return await GetByNameAsync(name) == null;
        }

        public Task<IdentityResult> UpdateAsync(Role model)
        {
            var result = _roleManager.UpdateAsync(model);
            return result;
        }
    }
}
