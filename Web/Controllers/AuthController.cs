using Microsoft.AspNetCore.Mvc;
using Business;
using Entity.DTOs;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserBusiness _userBusiness;

        public AuthController(UserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos.");

            var user = await _userBusiness.ValidateCredentialsAsync(loginDto.Email, loginDto.Password);

            if (user == null)
                return Unauthorized("Correo o contraseña incorrectos.");

            // Aquí simplemente devuelves los datos del usuario autenticado
            return Ok(new
            {
                message = "Inicio de sesión exitoso",
                user = user
            });
        }
    }
}
