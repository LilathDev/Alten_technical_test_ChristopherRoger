using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using back.Dtos;
using back.Models.Entities;
using back.Repositories;
using back.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasherService _passwordHasher;

        public AccountController(IUserRepository userRepository, IConfiguration configuration, IPasswordHasherService passwordHasher)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Create a new user account.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (userDto == null)
                return BadRequest(new { Message = "Invalid user data." });

            if (await _userRepository.UserExistsAsync(userDto.Username, userDto.Email))
                return Conflict(new { Message = "User with the same username or email already exists." });

            var hashedPassword = _passwordHasher.HashPassword(userDto.Password);

            var user = new User
            {
                Name = userDto.Name,
                Firstname = userDto.Firstname,
                Email = userDto.Email,
                Username = userDto.Username,
                PasswordHash = hashedPassword,
            };

            await _userRepository.AddUserAsync(user);

            return CreatedAtAction(nameof(Login), new { email = user.Email }, new { Message = "User created successfully.", User = user.Username });
        }

        /// <summary>
        /// Authenticate and generate a JWT token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
                return BadRequest(new { Message = "Invalid login data." });

            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
                return Unauthorized(new { Message = "Invalid email or password." });

            var isPasswordValid = _passwordHasher.VerifyPassword(user.PasswordHash, loginDto.Password);
            if (!isPasswordValid)
                return Unauthorized(new { Message = "Invalid email or password." });

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token, Message = "Login successful." });
        }

        /// <summary>
        /// Generate a JWT token for the authenticated user.
        /// </summary>
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Email == "admin@admin.com" ? "Admin" : "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}