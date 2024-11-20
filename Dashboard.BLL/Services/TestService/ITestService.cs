using Dashboard.DAL.ViewModels.Tests;

namespace Dashboard.BLL.Services.TestService
{
    public interface ITestService
    {
        public Task<ServiceResponse> CreateAsync(TestVM model, string userId);
        public Task<ServiceResponse> GetAllAsync();
        public Task<ServiceResponse> GetByUserAsync(string userId);
    }
}
