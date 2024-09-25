using Dashboard.BLL.Services.RoleService;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _roleService.GetAllAsync();
            return await GetResultAsync(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            var response = await _roleService.GetByNameAsync(name);
            return await GetResultAsync(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(RoleVM model)
        {
            if(string.IsNullOrEmpty(model.Name)) {
                return BadRequest("Роль повинна містити ім'я");
            }

            model.Id = Guid.NewGuid().ToString();

            var response = await _roleService.CreateAsync(model);
            return await GetResultAsync(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(RoleVM model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest("Роль повинна містити ім'я");
            }

            var response = await _roleService.UpdateAsync(model);
            return await GetResultAsync(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var response = await _roleService.DeleteAsync(id);
            return await GetResultAsync(response);
        }
    }
}
