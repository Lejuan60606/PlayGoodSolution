using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PlayGoodService.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlayGoodService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IOptions<JwtSettings> jwtSettings, ILogger<AuthController> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {           
            if (login.Username == "admin" && login.Password == "password")
            {
                var token = GenerateToken(login.Username);
                return Ok(new { Token = token });
            }
            else
            {
                _logger.LogWarning("Invalid login attempt.");
                return Unauthorized("Invalid credentials.");
            }
        }

        private string GenerateToken(string username)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
