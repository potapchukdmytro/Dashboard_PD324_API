using Dashboard.BLL.Services;
using Dashboard.BLL.Services.RoleService;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? id, string? name)
        {
            id = Request.Query[nameof(id)];
            name = Request.Query[nameof(name)];

            if(id == null && name == null)
            {
                var response = await _roleService.GetAllAsync();
                if(response.Success)
                {
                    return await GetResultAsync(response);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var response = await _roleService.GetByNameAsync(name);
                    if (response.Success)
                    {
                        return await GetResultAsync(response);
                    }
                }
                if (!string.IsNullOrWhiteSpace(id))
                {
                    var response = await _roleService.GetByIdAsync(id);
                    if (response.Success)
                    {
                        return await GetResultAsync(response);
                    }
                }
            }

            return await GetResultAsync(ServiceResponse.GetBadRequestResponse("Не вдалося отримати ролі"));
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
