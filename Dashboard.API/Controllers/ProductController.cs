using Dashboard.BLL.Services.ProductService;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(CreateProductVM model)
        {
            var response = await _productService.CreateAsync(model);
            return await GetResultAsync(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _productService.GetAllAsync();
            return await GetResultAsync(response);
        }
    }
}
