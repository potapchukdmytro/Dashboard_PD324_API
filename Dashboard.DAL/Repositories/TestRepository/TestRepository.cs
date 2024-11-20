using Dashboard.DAL.Models.Tests;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DAL.Repositories.TestRepository
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _context;

        public TestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Test model)
        {
            await _context.Tests.AddAsync(model);
            var result = await _context.SaveChangesAsync();
            return result != 0;
        }

        public IQueryable<Test> GetAll()
        {
            return _context.Tests.AsNoTracking();
        }
    }
}
