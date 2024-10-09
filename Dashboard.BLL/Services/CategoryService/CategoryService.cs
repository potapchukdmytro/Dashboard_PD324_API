using Dashboard.DAL.Models;
using Dashboard.DAL.Repositories.CategoryRepository;
using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.Services.CategoryService
{
    public class CategoryService : ICategoryService
    { 
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse> CreateAsync(CreateCategoryVM model)
        {
            if(await _categoryRepository.IsExistsAsync(model.Name))
            {
                return ServiceResponse.GetBadRequestResponse($"Категорія {model.Name} вже існує");
            }

            var newCategory = new Category
            {
                Name = model.Name,
                Id = Guid.NewGuid(),
                NormalizedName = model.Name.ToUpper()
            };

            var result = await _categoryRepository.CreateAsync(newCategory);

            if(result)
            {
                return ServiceResponse.GetOkResponse("Категорія успішно додадана");
            }
            else
            {
                return ServiceResponse.GetOkResponse("Не вдалося додати категорію");
            }
        }
    }
}
