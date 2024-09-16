using Dashboard.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected Task<IActionResult> GetResultAsync(ServiceResponse serviceResponse)
        {
            return Task.FromResult<IActionResult>(StatusCode((int)serviceResponse.StatusCode, serviceResponse));
        }
    }
}
