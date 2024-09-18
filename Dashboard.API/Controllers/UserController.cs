﻿using Dashboard.BLL.Services.UserService;
using Dashboard.BLL.Validators;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MimeKit.Encodings;
using System.Buffers.Text;
using System.Text;

namespace Dashboard.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        //[HttpPost("imageTest")]
        //public async Task<IActionResult> ImageTest([FromBody] string base64)
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _userService.GetAllUsersAsync();

            return await GetResultAsync(response);
        }

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
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var response = await _userService.GetByIdAsync(id);
            return await GetResultAsync(response);
        }
    }
}
