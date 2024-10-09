using Dashboard.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DAL.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Category model)
        {
            await _context.Categories.AddAsync(model);
            var result = await _context.SaveChangesAsync();
            return result != 0;
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.NormalizedName == name.ToUpper());
        }

        public async Task<bool> IsExistsAsync(string name)
        {
            var category = await GetByNameAsync(name);
            return category != null;
        }
    }
}
