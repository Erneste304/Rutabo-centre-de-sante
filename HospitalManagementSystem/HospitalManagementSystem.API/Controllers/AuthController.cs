using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Core.Services;
using HospitalManagementSystem.Core.Models;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.AuthenticateAsync(request.Username, request.Password);
            
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            // In production, generate JWT token here
            return Ok(new { 
                Id = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                UserType = user.UserType.ToString(),
                Email = user.Email,
                DoctorId = user.DoctorId,
                PatientId = user.PatientId,
                Status = user.IsActive ? "Active" : "Inactive",
                CreatedAt = user.CreatedAt,
                LastLogin = DateTime.UtcNow
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _userService.UserExistsAsync(request.Username, request.Email))
                return BadRequest(new { message = "Username or email already exists" });

            var user = new User
            {
                Username = request.Username,
                FullName = request.FullName,
                Email = request.Email,
                UserType = request.UserType
            };

            await _userService.RegisterAsync(user, request.Password);
            
            return Ok(new { message = "Registration successful" });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userService.GetByUsernameAsync(request.Username);
            if (user != null)
            {
                user.ResetRequested = true;
                await _userService.UpdateUserAsync(user);
            }
            
            return Ok(new { message = "If the account exists, a reset request has been sent to the administrator." });
        }
    }

    public class ForgotPasswordRequest
    {
        public string Username { get; set; } = string.Empty;
    }

    public class LoginRequest

    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserType UserType { get; set; }
    }
}