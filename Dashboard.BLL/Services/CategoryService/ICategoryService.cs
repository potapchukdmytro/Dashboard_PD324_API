using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ServiceResponse> CreateAsync(CreateCategoryVM model);
    }
}
