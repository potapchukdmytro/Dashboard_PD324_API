using Dashboard.BLL.Services.UserService;
using Dashboard.BLL.Validators;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _userService.GetAllUsersAsync();

            return await GetResultAsync(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(UserVM model)
        {
            var validator = new UserValidator();
            var validationResult = await validator.ValidateAsync(model);

            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _userService.UpdateAsync(model);

            return await GetResultAsync(response);
        }
    }
}
