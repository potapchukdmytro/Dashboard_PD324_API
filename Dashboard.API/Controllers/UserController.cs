using Dashboard.BLL.Services;
using Dashboard.BLL.Services.UserService;
using Dashboard.BLL.Validators;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        //[HttpPost("SaveImageBase64")]
        //public async Task<IActionResult> SaveImageBase64([FromBody] string base64)
        //{
        //    var splitBase64 = base64.Split(",");
        //    var base64Valid = splitBase64[1];

        //    var part1 = splitBase64[0].Split(";")[0];
        //    var ext = part1.Substring(part1.IndexOf('/') + 1);
        //    var image = Convert.FromBase64String(base64Valid);
        //    var root = _webHostEnvironment.WebRootPath;

        //    System.IO.File.WriteAllBytes(Path.Combine(root, "image." + ext), image);

        //    return Ok();
        //}

        [HttpPost("ImageFromUser")]
        public async Task<IActionResult> AddImageFromUserAsync([FromForm] UserImageVM model)
        {
            if (model.Image == null)
            {
                return BadRequest("Зображення не знайдено");
            }

            if (string.IsNullOrEmpty(model.UserId))
            {
                return BadRequest("Невірно вказано id користувача");
            }

            var response = await _userService.AddImageFromUserAsync(model);

            return await GetResultAsync(response);
        }

        //[HttpGet("GetAll")]
        //public async Task<IActionResult> GetAllAsync()
        //{
        //    var response = await _userService.GetAllUsersAsync();

        //    return await GetResultAsync(response);
        //}

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UserVM model)
        {
            var validator = new UserValidator();
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _userService.UpdateAsync(model);

            return await GetResultAsync(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserVM model)
        {
            var validator = new CreateUserValidator();

            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _userService.CreateAsync(model);

            return await GetResultAsync(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var response = await _userService.DeleteAsync(id);
            return await GetResultAsync(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string? id, string? userName, string? email)
        {
            id = Request.Query[nameof(id)];
            userName = Request.Query[nameof(userName)];
            email = Request.Query[nameof(email)];

            if (id == null && userName == null && email == null)
            {
                var response = await _userService.GetAllUsersAsync();
                return await GetResultAsync(response);
            }

            if (!string.IsNullOrEmpty(id))
            {
                var response = await _userService.GetByIdAsync(id);

                if (response.Success)
                {
                    return await GetResultAsync(response);
                }
            }
            if (!string.IsNullOrEmpty(userName))
            {
                var response = await _userService.GetByUserNameAsync(userName);

                if (response.Success)
                {
                    return await GetResultAsync(response);
                }
            }
            if (!string.IsNullOrEmpty(email))
            {
                var response = await _userService.GetByEmailAsync(email);

                if (response.Success)
                {
                    return await GetResultAsync(response);
                }
            }

            return await GetResultAsync(ServiceResponse.GetBadRequestResponse("Користувача не знайдено", errors: "Користувача не знайдено"));
        }
    }
}
