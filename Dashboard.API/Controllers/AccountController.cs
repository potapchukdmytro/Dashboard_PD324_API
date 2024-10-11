using Dashboard.BLL.Services;
using Dashboard.BLL.Services.AccountService;
using Dashboard.BLL.Services.JwtService;
using Dashboard.BLL.Validators;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IJwtService _jwtService;

        public AccountController(IAccountService accountService, IJwtService jwtService)
        {
            _accountService = accountService;
            _jwtService = jwtService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpVM model)
        {
            var validator = new SignUpValidator();
            var validateResult = await validator.ValidateAsync(model);

            var response = await _accountService.SignUpAsync(model);
            return await GetResultAsync(response);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync(SignInVM model)
        {
            var validator = new SignInValidator();
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var response = await _accountService.SignInAsync(model);

            return await GetResultAsync(response);
        }

        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmationAsync(string? u, string? t)
        {
            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(t))
            {
                return NotFound();
            }

            var response = await _accountService.EmailConfirmationAsync(u, t);

            if (response.Success)
            {
                return Redirect("http://localhost:3000");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Пошта не може бути порожньою");
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
            {
                return NotFound("Невірний формат пошти");
            }

            var response = await _accountService.ForgotPasswordAsync(email);

            return await GetResultAsync(response);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromForm] ResetPasswordVM model)
        {
            var validator = new ResetPasswordValidator();
            var result = await validator.ValidateAsync(model);

            if (!result.IsValid)
            {
                return NotFound();
            }

            var response = await _accountService.ResetPasswordAsync(model);

            if (response.Success)
            {
                return Redirect("https://mydashboard.com/signin");
            }

            return NotFound();
        }

        [HttpPost("RefreshTokens")]
        public async Task<IActionResult> RefreshTokensAsync([FromBody] JwtVM model)
        {
            if(string.IsNullOrEmpty(model.RefreshToken) ||
                string.IsNullOrEmpty(model.AccessToken))
            {
                return await GetResultAsync(ServiceResponse.GetBadRequestResponse("Invalid tokens", errors: "Invalid tokens"));
            }

            var response = await _jwtService.RefreshTokensAsync(model.RefreshToken, model.AccessToken);
            return await GetResultAsync(response);
        }
    }
}
