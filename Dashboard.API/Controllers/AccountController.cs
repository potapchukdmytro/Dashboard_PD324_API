using Dashboard.BLL.Services.AccountService;
using Dashboard.BLL.Validators;
using Dashboard.DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpVM model)
        {
            var validator = new SignUpValidator();
            var validateResult = await validator.ValidateAsync(model);

            if(validateResult.IsValid)
            {
                var response = await _accountService.SignUpAsync(model);

                if(response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response); 
                }
            }
            else
            {
                return BadRequest(validateResult.Errors);
            }
        }

        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmationAsync(string? u, string? t)
        {
            if(string.IsNullOrEmpty(u) || string.IsNullOrEmpty(t))
            {
                return NotFound();
            }



            return Ok("Пошта підтверджена");
        }
    }
}
