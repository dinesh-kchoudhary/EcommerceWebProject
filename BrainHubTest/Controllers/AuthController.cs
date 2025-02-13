using BrainHubTest.Data;
using BrainHubTest.Model;
using BrainHubTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrainHubTest.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService) { _userService = userService; }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new ApiResponse<string>(false, "Invalid request", null));
            }

            var token = await _userService.AuthenticateAsync(request.Email, request.Password);

            if (token == null)
            {
                return Unauthorized(new ApiResponse<string>(false, "Invalid email or password", null));
            }

            return Ok(new ApiResponse<string>(true, "Login successful", token));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new ApiResponse<string>(false, "Invalid request", null));
            }

            var response = await _userService.RegisterAsync(request.Email, request.Password);

            if (!response.Success)
            {
                return BadRequest(new ApiResponse<string>(false, "Email already in use", null));
            }

            return Ok(response);
        }

    }
}
