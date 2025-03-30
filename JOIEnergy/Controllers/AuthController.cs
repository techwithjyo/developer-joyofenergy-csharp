using JOIEnergy.Services;
using Microsoft.AspNetCore.Mvc;

namespace JOIEnergy.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly TokenService _tokenService;
        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)  
        {
            // Validate the user credentials (this is just a simple example)
            if (model.Username == "test" && model.Password == "password")
            {
                var token = _tokenService.GenerateToken(model.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
