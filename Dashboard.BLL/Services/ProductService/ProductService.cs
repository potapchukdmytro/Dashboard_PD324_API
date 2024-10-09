using AutoMapper;
using Dashboard.BLL.Services.CategoryService;
using Dashboard.DAL.Models;
using Dashboard.DAL.Repositories.CategoryRepository;
using Dashboard.DAL.Repositories.ProductRepository;
using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse> CreateAsync(CreateProductVM model)
        {
            var category = await _categoryRepository.GetByNameAsync(model.Category);

            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Image = null,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                Name = model.Name,
                Price = model.Price,
                CategoryId = category.Id
            };

            var result = await _productRepository.CreateAsync(newProduct);

            if(result)
            {
                return ServiceResponse.GetOkResponse("Продукт успішно додано");
            }
            else
            {
                return ServiceResponse.GetBadRequestResponse("Не вдалося додати продукт");
            }
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            var models = _mapper.Map<List<ProductVM>>(products);

            return ServiceResponse.GetOkResponse("Список категорій", models);
        }
    }
}
