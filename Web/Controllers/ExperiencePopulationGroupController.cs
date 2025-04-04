using Business;
using Data;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace Web
{/// <summary>
 /// Controlador para la gestión de grupos de población con experiencia en el sistema.
 /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ExperiencePopulationGroupController : ControllerBase
    {
        private readonly ExperiencePopulationBusiness _ExperiencePopulationGroupBusiness;
        private readonly ILogger<ExperiencePopulationGroupController> _logger;

        /// <summary>
        /// Constructor controlador de grupos de población con experiencia.
        /// </summary>
        /// <param name="ExperiencePopulationGroupBusiness">Capa de negocios de grupos de población con experiencia.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public ExperiencePopulationGroupController(ExperiencePopulationBusiness ExperiencePopulationGroupBusiness, ILogger<ExperiencePopulationGroupController> logger)
        {
            _ExperiencePopulationGroupBusiness = ExperiencePopulationGroupBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los grupos de población con experiencia en el sistema.
        /// </summary>
        /// <returns>Lista de grupos de población con experiencia</returns>
        /// <response code="200">Lista de grupos de población con experiencia</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExperiencePopulationData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllExperiencePopulationGroups()
        {
            try
            {
                var groups = await _ExperiencePopulationGroupBusiness.GetAllExperiencePopulationsAsync();
                return Ok(groups);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los grupos de población con experiencia");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un grupo de población con experiencia por su ID.
        /// </summary>
        /// <param name="id">ID del grupo de población con experiencia</param>
        /// <returns>Grupo de población con experiencia solicitado</returns>
        /// <response code="200">Retorna el grupo de población con experiencia solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Grupo de población con experiencia no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExperiencePopulationGroupDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetExperiencePopulationGroupById(int id)
        {
            try
            {
                var group = await _ExperiencePopulationGroupBusiness.GetExperiencePopulationByIdAsync(id);
                return Ok(group);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el grupo de población con experiencia con ID: {ExperiencePopulationGroupId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Grupo de población con experiencia no encontrado con ID: {ExperiencePopulationGroupId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener grupo de población con experiencia con ID: {ExperiencePopulationGroupId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un grupo de población con experiencia en el sistema.
        /// </summary>
        /// <param name="group">Datos del grupo de población con experiencia a crear</param>
        /// <returns>Grupo de población con experiencia creado</returns>
        /// <response code="201">Retorna el grupo de población con experiencia creado</response>
        /// <response code="400">Datos del grupo de población con experiencia no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ExperiencePopulationGroupDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateExperiencePopulationGroup([FromBody] ExperiencePopulationGroupDTO group)
        {
            try
            {
                var createdGroup = await _ExperiencePopulationGroupBusiness.CreateExperiencePopulationAsync(group);
                return CreatedAtAction(nameof(GetExperiencePopulationGroupById), new { id = createdGroup.Id }, createdGroup);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear grupo de población con experiencia");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear grupo de población con experiencia");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
