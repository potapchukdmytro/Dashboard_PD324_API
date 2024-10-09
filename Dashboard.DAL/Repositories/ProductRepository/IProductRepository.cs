using Dashboard.DAL.Models;

namespace Dashboard.DAL.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        Task<bool> CreateAsync(Product model);
        Task<List<Product>> GetAllAsync();
    }
}
