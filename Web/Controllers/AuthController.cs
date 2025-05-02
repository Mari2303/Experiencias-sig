using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Entity.DTOs;

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
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userBusiness.ValidateCredentialsAsync(loginDto.Email, loginDto.Password);
            if (user == null)
                return BadRequest("Credenciales inválidas");

            return Ok(user);
        }

    }
}

