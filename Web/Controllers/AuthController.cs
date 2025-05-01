using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business;
using Entity;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserBusiness _userBusiness;
        private readonly IConfiguration _configuration;

        public AuthController(UserBusiness userBusiness, IConfiguration configuration)
        {
            _userBusiness = userBusiness;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userBusiness.LoginAsync(request.Email, request.Password);

            if (user == null)
                return Unauthorized(new { message = "Credenciales incorrectas" });

            var claims = new[]
 {
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Name, user.Person.Name), // Corregido para usar user.Person.Name
};


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperClaveJWT2025$%&"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "tuapp.com",
                audience: "tuapp.com",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                user = new
                {
                    user.Id,
                    user.Person.Name,
                    user.Email
                }
            });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetUserData()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(new { message = $"Hola {email}, estás autenticado." });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

