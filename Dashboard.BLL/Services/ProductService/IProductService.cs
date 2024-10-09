using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse> CreateAsync(CreateProductVM model);
        Task<ServiceResponse> GetAllAsync();
    }
}
