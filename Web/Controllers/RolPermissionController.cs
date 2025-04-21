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
    public class RolPermissionController : ControllerBase
    {
        private readonly RolPermisionBusiness _RolPermissionBusiness;
        private readonly ILogger<RolPermissionController> _logger;

        /// <summary>
        /// Constructor controlador de permisos.
        /// </summary>
        /// <param name="RolPermissionBusiness">Capa de negocios de permisos.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public RolPermissionController(RolPermisionBusiness RolPermissionBusiness, ILogger<RolPermissionController> logger)
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
        [ProducesResponseType(typeof(IEnumerable<RolPermissionData>), 200)]
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
        [ProducesResponseType(typeof(RolPermissionData), 200)]
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
        [ProducesResponseType(typeof(RolPermissionData), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRolPermission([FromBody] RolPermissionDTO RolPermission)
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
        public async Task<IActionResult> PatchRolPermission(int id, [FromBody] RolPermissionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _RolPermissionBusiness.PatchRolPermissionAsync(id, dto.RolId, dto.PermissionId);
                if (!updated)
                    return NotFound(new { message = "RolPermission no encontrado" });

                return Ok(new
                {
                    message = "RolPermission actualizado correctamente",
                    id,
                    rolId = dto.RolId,
                    rolName = dto.RolName,
                    permissionId = dto.PermissionId,
                    permissionName = dto.PermissionName
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
        public async Task<IActionResult> PutRolPermission(int id, [FromBody] RolPermissionDTO dto)
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
                    rolName = dto.RolName,
                    permissionId = dto.PermissionId,
                    permissionName = dto.PermissionName
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
















    }
}