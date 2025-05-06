using Business;
using Entity.DTOs.Form;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Utilities.Exeptions;
using ValidationException = Utilities.Exeptions.ValidationException;

namespace Web.Controllers
{
    /// <summary>
    /// Controlador para la gestión de formularios en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FromController : ControllerBase
    {
        private readonly FromBusiness _FormBusiness;
        private readonly ILogger<FromController> _logger;

        /// <summary>
        /// Constructor del controlador de formularios
        /// </summary>
        /// <param name="formBusiness">Capa de negocio de formularios</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public FromController(FromBusiness formBusiness, ILogger<FromController> logger)
        {
            _FormBusiness = formBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los formularios del sistema
        /// </summary>
        /// <returns>Lista de formularios</returns>
        /// <response code="200">Retorna la lista de formularios</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FormDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllForms()
        {
            try
            {
                var forms = await _FormBusiness.GetAllFormsAsync();
                return Ok(forms);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener formularios");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un formulario específico por su ID
        /// </summary>
        /// <param name="id">ID del formulario</param>
        /// <returns>Formulario solicitado</returns>
        /// <response code="200">Retorna el formulario solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Formulario no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFormById(int id)
        {
            try
            {
                var form = await _FormBusiness.GetFormByIdAsync(id);
                return Ok(form);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el formulario con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Formulario no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener formulario con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Crea un nuevo formulario en el sistema
        /// </summary>
        /// <param name="formDto">Datos del formulario a crear</param>
        /// <returns>Formulario creado</returns>
        /// <response code="201">Retorna el formulario creado</response>
        /// <response code="400">Datos del formulario no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(FormDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateForm([FromBody] FormDTO formDto)
        {
            try
            {
                var createdForm = await _FormBusiness.CreateFormAsync(formDto);
                return CreatedAtAction(nameof(GetFormById), new { id = createdForm.Id }, createdForm);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear formulario");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear formulario");
                return StatusCode(500, new { message = ex.Message });
            }
        }
        /// <summary>
        /// Elimina un formulario por su ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteForm(int id)
        {
            try
            {
                var result = await _FormBusiness.DeleteFormAsync(id);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "ID inválido al intentar eliminar: {Id}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Formulario no encontrado al eliminar: {Id}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al eliminar formulario");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza completamente un formulario existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateForm(int id, [FromBody] FormDTO dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "El id de la ruta no coincide con el del cuerpo" });
            try
            {
                var result = await _FormBusiness.UpdateFormAsync(dto);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación al actualizar formulario");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Formulario no encontrado al actualizar: {Id}", dto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al actualizar formulario");
                return StatusCode(500, new { message = ex.Message });
            }
        }
        /// <summary>
        /// Actualiza parcialmente un formulario (solo nombre y descripción).
        /// </summary>
        [HttpPatch]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePartialForm([FromBody] FormDTO dto)
        {
            try
            {
                var result = await _FormBusiness.UpdateParcialFormAsync(dto);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación en actualización parcial");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Formulario no encontrado en actualización parcial: {Id}", dto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error en actualización parcial de formulario");
                return StatusCode(500, new { message = ex.Message });
            }
        }
        /// <summary>
        /// Cambia el estado activo/inactivo de un formulario.
        /// </summary>
        [HttpDelete("active")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SetFormActive([FromBody] FormDTO dto)
        {
            try
            {
                var result = await _FormBusiness.SetFormActiveAsync(dto);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Error de validación al cambiar estado");
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Formulario no encontrado al cambiar estado: {Id}", dto.Id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al cambiar estado activo");
                return StatusCode(500, new { message = ex.Message });
            }
        }



    }
}