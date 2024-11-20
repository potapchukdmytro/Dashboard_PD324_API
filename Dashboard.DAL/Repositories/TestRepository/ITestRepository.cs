using Dashboard.DAL.Models.Tests;

namespace Dashboard.DAL.Repositories.TestRepository
{
    public interface ITestRepository
    {
        Task<bool> CreateAsync(Test model);
        IQueryable<Test> GetAll();
    }
}
