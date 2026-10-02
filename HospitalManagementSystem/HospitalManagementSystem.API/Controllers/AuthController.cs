using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
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
    [AllowAnonymous]
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

            if (user.UserType == UserType.Patient)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    message = "Patient portal access is not available until patient-specific data permissions are implemented."
                });
            }

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

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]
                    ?? throw new InvalidOperationException("JWT signing key is not configured.")));
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
            if (request.UserType != UserType.Patient)
                return BadRequest(new { message = "Public registration is only available for patient accounts." });

            if (await _userService.UserExistsAsync(request.Username, request.Email))
                return BadRequest(new { message = "Username or email already exists" });

            var user = new User
            {
                Username = request.Username,
                FullName = request.FullName,
                Email = request.Email,
                UserType = UserType.Patient
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
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(128, MinimumLength = 12)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public UserType? UserType { get; set; }
    }
}