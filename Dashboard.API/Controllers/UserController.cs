using Dashboard.DAL;
using Dashboard.DAL.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dashboard.API.Controllers
{  

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var users = _userManager.Users.AsEnumerable();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
