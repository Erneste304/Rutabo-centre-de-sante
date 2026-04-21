using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Core.Services;
using HospitalManagementSystem.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.AuthenticateAsync(request.Username, request.Password);
            
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            var token = GenerateJwtToken(user);

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
                LastLogin = DateTime.UtcNow,
                Token = token
            });
        }

        private string GenerateJwtToken(dynamic user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "ThisIsMySecretKeyForHospitalManagementSystem2024"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
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