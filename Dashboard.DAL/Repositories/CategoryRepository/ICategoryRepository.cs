using Dashboard.DAL.Models;

namespace Dashboard.DAL.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<bool> IsExistsAsync(string name);
        Task<bool> CreateAsync(Category model); 
        Task<Category?> GetByNameAsync(string name);
    }
}
