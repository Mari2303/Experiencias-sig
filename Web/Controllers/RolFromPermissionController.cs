using Business;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;



namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de permisos en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RolFromPermissionController : ControllerBase
    {
        private readonly RolFromPermisionBusiness _RolPermissionBusiness;
        private readonly ILogger<RolFromPermissionController> _logger;

        /// <summary>
        /// Constructor controlador de permisos.
        /// </summary>
        /// <param name="RolPermissionBusiness">Capa de negocios de permisos.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public RolFromPermissionController(RolFromPermisionBusiness RolPermissionBusiness, ILogger<RolFromPermissionController> logger)
        {
            _RolPermissionBusiness = RolPermissionBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los permisos del sistema.
        /// </summary>
        /// <returns>Lista de permisos</returns>
        /// <response code="200">Lista de permisos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RolFromPermissionData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllRolPermissions()
        {
            try
            {
                var RolPermissions = await _RolPermissionBusiness.GetAllRolPermissionsAsync();
                return Ok(RolPermissions);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un permiso por su ID.
        /// </summary>
        /// <param name="id">ID del permiso</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el permiso solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RolFromPermissionData), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRolPermissionById(int id)
        {
            try
            {
                var RolPermission = await _RolPermissionBusiness.GetAllRolPermissionsAsync();
                return Ok(RolPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {RolPermissionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {RolPermissionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {RolPermissionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un permiso en el sistema.
        /// </summary>
        /// <param name="RolPermission">Datos del permiso a crear</param>
        /// <returns>Permiso creado</returns>
        /// <response code="201">Retorna el permiso creado</response>
        /// <response code="400">Datos del permiso no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(RolFromPermissionData), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRolPermission([FromBody] RolFromPermissionDTO RolPermission)
        {
            try
            {
                var createdRolPermission = await _RolPermissionBusiness.CreateRolPermisionAsync(RolPermission);
                return CreatedAtAction(nameof(GetRolPermissionById), new { id = createdRolPermission.Id }, createdRolPermission);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear permiso");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear permiso");
                return StatusCode(500, new { message = ex.Message });
            }
        }



        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PatchRolPermission(int id, [FromBody] RolFromPermissionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _RolPermissionBusiness.PatchRolPermissionAsync(id, dto.RolId, dto.PermissionId, dto.FromId);
                if (!updated)
                    return NotFound(new { message = "RolPermission no encontrado" });

                return Ok(new
                {
                    message = "RolPermission actualizado correctamente",
                    id,
                    rolId = dto.RolId,
                    From = dto.FromId,
                    permissionId = dto.PermissionId
                   
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new { message = "RolPermission no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PatchRolPermission");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutRolPermission(int id, [FromBody] RolFromPermissionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _RolPermissionBusiness.PutRolPermissionAsync(id, dto);
                if (!updated)
                    return NotFound(new { message = "RolPermission no encontrado" });

                return Ok(new
                {
                    message = "RolPermission actualizado correctamente",
                    id,
                    rolId = dto.RolId,
                    FromId = dto.FromId,
                    permissionId = dto.PermissionId
                    
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new { message = "RolPermission no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PutRolPermission");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }



        /// <summary>
        /// Elimina permanentemente un RolPermission.
        /// </summary>
        /// <param name="id">ID del RolPermission a eliminar.</param>
        /// <returns>Código de estado HTTP.</returns>
        [HttpDelete("permanent/{id}")]
        public async Task<IActionResult> DeletePermanent(int id)
        {
            var result = await _RolPermissionBusiness.DeletePermanentAsync(id);
            if (!result)
                return NotFound($"RolPermission con ID {id} no encontrado o error al eliminar.");
            return NoContent();
        }












    }
}