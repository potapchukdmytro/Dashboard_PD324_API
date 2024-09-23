using Dashboard.BLL.Services.RoleService;
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
    }
}
