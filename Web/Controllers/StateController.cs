using Business;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;


namespace Web
{
    /// <summary>
    /// Controlador para la gestión de estados en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StateController : ControllerBase
    {
        private readonly StateBusiness _stateBusiness;
        private readonly ILogger<StateController> _logger;

        /// <summary>
        /// Constructor controlador de estados.
        /// </summary>
        /// <param name="stateBusiness">Capa de negocios de estados.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public StateController(StateBusiness stateBusiness, ILogger<StateController> logger)
        {
            _stateBusiness = stateBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtienes todos los estados del sistema.
        /// </summary>
        /// <returns>Lista de estados</returns>
        /// <response code="200">Lista de estados</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StateData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllStates()
        {
            try
            {
                var states = await _stateBusiness.GetAllStatesAsync();
                return Ok(states);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los estados");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un estado por su ID.
        /// </summary>
        /// <param name="id">ID del estado</param>
        /// <returns>Estado solicitado</returns>
        /// <response code="200">Retorna el estado solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Estado no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StateDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetStateById(int id)
        {
            try
            {
                var state = await _stateBusiness.GetStateByIdAsync(id);
                return Ok(state);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el estado con ID: {StateId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Estado no encontrado con ID: {StateId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener estado con ID: {StateId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un estado en el sistema.
        /// </summary>
        /// <param name="state">Datos del estado a crear</param>
        /// <returns>Estado creado</returns>
        /// <response code="201">Retorna el estado creado</response>
        /// <response code="400">Datos del estado no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(StateDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateState([FromBody] StateDTO state)
        {
            try
            {
                var createdState = await _stateBusiness.CreateStateAsync(state);
                return CreatedAtAction(nameof(GetStateById), new { id = createdState.Id }, createdState);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear estado");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear estado");
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchState(int id, [FromBody] StateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _stateBusiness.PatchStateAsync(id, dto.Name, dto.HistoryExperienceId, dto.Active);
                return Ok(new { message = "Estado actualizado correctamente", id });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new { message = "Estado no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PatchState");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutState(int id, [FromBody] StateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _stateBusiness.PutStateAsync(id, dto);
                return Ok(new { message = "Estado actualizado correctamente", id });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new { message = "Estado no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PutState");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            try
            {
                await _stateBusiness.DeleteStateAsync(id);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new { message = "Estado no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en DeleteState");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }











    }
}
