using Dashboard.BLL.Services;
using Dashboard.BLL.Services.TestService;
using Dashboard.DAL.ViewModels.Tests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dashboard.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/tests")]
    public class TestController : BaseController
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]string? userId)
        {
            userId = Request.Query[nameof(userId)];

            if (userId == null)
            {
                var response = await _testService.GetAllAsync();
                return await GetResultAsync(response);
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] TestVM model)
        {
            if(User == null)
            {
                return await GetResultAsync(ServiceResponse.GetServiceResponse("Ви не авторизовані", false, null, HttpStatusCode.Unauthorized));
            }

            var userId = User.Claims.SingleOrDefault(c => c.Type == "id").Value;

            if(userId  == null)
            {
                if (User == null)
                {
                    return await GetResultAsync(ServiceResponse.GetServiceResponse("Error", false, null, HttpStatusCode.Forbidden));
                }
            }

            var response = await _testService.CreateAsync(model, userId);
            return await GetResultAsync(response);
        }
    }
}
