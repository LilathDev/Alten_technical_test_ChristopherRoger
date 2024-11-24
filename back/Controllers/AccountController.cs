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
    [Route("api/account")]
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

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            if (await _userRepository.UserExistsAsync(userDto.Username, userDto.Email))
            {
                return BadRequest("User already exists.");
            }

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

            return Ok("User created successfully.");
        }

        [HttpPost("token")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var isPasswordValid = _passwordHasher.VerifyPassword(user.PasswordHash, loginDto.Password);
            if (!isPasswordValid)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Générez et retournez le token JWT ici
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}