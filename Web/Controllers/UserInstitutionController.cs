using Microsoft.AspNetCore.Mvc;
using static BusinessException.BusinessRuleException;

namespace Web
{
    /// <summary>
    /// Controlador para la gestión de instituciones de usuarios en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserInstitutionController : ControllerBase
    {
        private readonly UserInstitutionBusiness _UserInstitutionBusiness;
        private readonly ILogger<UserInstitutionController> _logger;

        /// <summary>
        /// Constructor controlador de instituciones de usuarios.
        /// </summary>
        /// <param name="UserInstitutionBusiness">Capa de negocios de instituciones de usuarios.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public UserInstitutionController(UserInstitutionBusiness UserInstitutionBusiness, ILogger<UserInstitutionController> logger)
        {
            _UserInstitutionBusiness = UserInstitutionBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las instituciones de usuarios en el sistema.
        /// </summary>
        /// <returns>Lista de instituciones de usuarios</returns>
        /// <response code="200">Lista de instituciones de usuarios</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserInstitutionData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUserInstitutions()
        {
            try
            {
                var institutions = await _UserInstitutionBusiness.GetAllUserInstitutionsAsync();
                return Ok(institutions);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las instituciones de usuarios");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene una institución de usuario por su ID.
        /// </summary>
        /// <param name="id">ID de la institución de usuario</param>
        /// <returns>Institución de usuario solicitada</returns>
        /// <response code="200">Retorna la institución de usuario solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Institución de usuario no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserInstitutionDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserInstitutionById(int id)
        {
            try
            {
                var institution = await _UserInstitutionBusiness.GetUserInstitutionByIdAsync(id);
                return Ok(institution);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la institución de usuario con ID: {UserInstitutionId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Institución de usuario no encontrada con ID: {UserInstitutionId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener institución de usuario con ID: {UserInstitutionId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una institución de usuario en el sistema.
        /// </summary>
        /// <param name="institution">Datos de la institución de usuario a crear</param>
        /// <returns>Institución de usuario creada</returns>
        /// <response code="201">Retorna la institución de usuario creada</response>
        /// <response code="400">Datos de la institución de usuario no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserInstitutionDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateUserInstitution([FromBody] UserInstitutionDto institution)
        {
            try
            {
                var createdInstitution = await _UserInstitutionBusiness.CreateUserInstitutionAsync(institution);
                return CreatedAtAction(nameof(GetUserInstitutionById), new { id = createdInstitution.Id }, createdInstitution);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear institución de usuario");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear institución de usuario");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
