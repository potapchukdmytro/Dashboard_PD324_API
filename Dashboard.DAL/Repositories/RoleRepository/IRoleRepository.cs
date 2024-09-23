using Dashboard.DAL.Models.Identity;

namespace Dashboard.DAL.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllAsync();
    }
}
