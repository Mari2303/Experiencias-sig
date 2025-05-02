using Business;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Utilities.Exeptions;

namespace Web
{/// <summary>
 /// Controlador para la gestión de usuarios en el sistema.
 /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly UserBusiness _UserBusiness;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Constructor controlador de usuarios.
        /// </summary>
        /// <param name="UserBusiness">Capa de negocios de usuarios.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public UserController(UserBusiness UserBusiness, ILogger<UserController> logger)
        {
            _UserBusiness = UserBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los usuarios en el sistema.
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        /// <response code="200">Lista de usuarios</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _UserBusiness.GetAllUserAsync();
                return Ok(users);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los usuarios");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <returns>Usuario solicitado</returns>
        /// <response code="200">Retorna el usuario solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Usuario no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _UserBusiness.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el usuario con ID: {UserId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Usuario no encontrado con ID: {UserId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener usuario con ID: {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un usuario en el sistema.
        /// </summary>
        /// <param name="user">Datos del usuario a crear</param>
        /// <returns>Usuario creado</returns>
        /// <response code="201">Retorna el usuario creado</response>
        /// <response code="400">Datos del usuario no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
        {
            try
            {
                var createdUser = await _UserBusiness.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear usuario");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                return StatusCode(500, new { message = ex.Message });
            }
        }




        // PATCH
        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PatchUser(int id, [FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _UserBusiness.UpdatePartialAsync(id,  dto.Email, dto.Password, dto.Active, dto.PersonId);
                if (!updated)
                    return NotFound(new { message = "Usuario no encontrado" });

                return Ok(new { message = "Usuario actualizado correctamente", id = id });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PatchUser");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        // PUT
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutUser(int id, [FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _UserBusiness.UpdateFullAsync(id, dto);
                if (!updated)
                    return NotFound(new { message = "Usuario no encontrado" });

                return Ok(new { message = "Usuario actualizado correctamente", id = id });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PutUser");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        // DELETE lógico
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _UserBusiness.DeleteLogicalAsync(id);
                return NoContent(); // 204
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar (lógicamente) el usuario");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }




        [HttpDelete("delete/{id}/Permanente")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _UserBusiness.DeleteAsync(id);
                if (!result)
                    return NotFound($"Usuario con ID {id} no encontrado.");

                return Ok($"Usuario con ID {id} eliminado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el usuario con ID {id}");
                return StatusCode(500, "Error interno del servidor.");
            }
        }


       







    }
}
