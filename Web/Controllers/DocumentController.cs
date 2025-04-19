using Business;
using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exeptions;


namespace Web
{
    /// <summary>
    /// Controlador para la gestion de documentos en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentBusiness _DocumentBusiness;
        private readonly ILogger<DocumentController> _logger;

        /// <summary>
        /// Constructor controlador de documentos.
        /// </summary>
        /// <param name="DocumentBusiness">Capa de negocios de documentos.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public DocumentController(DocumentBusiness DocumentBusiness, ILogger<DocumentController> logger)
        {
            _DocumentBusiness = DocumentBusiness;
            _logger = logger;
        }
        /// <summary>
        /// Obtienes todos los documentos del sistema.
        /// </summary>
        /// <returns>Lista de documentos</returns>
        /// <response code="200">Lista de documentos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DocumentData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllDocuments()
        {
            try
            {
                var documents = await _DocumentBusiness.GetAllDocumentsAsync();
                return Ok(documents);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener los documentos");
                return StatusCode(500, new { message = ex.Message });
            }
        }
        /// <summary>
        /// Obtiene un documento por su ID.
        /// </summary>
        /// <param name="id">ID del documento</param>
        /// <returns>Documento solicitado</returns>
        /// <response code="200">Retorna el documento solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Documento no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DocumentDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            try
            {
                var document = await _DocumentBusiness.GetDocumentByIdAsync(id);
                return Ok(document);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el documento con ID: {DocumentId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Documento no encontrado con ID: {DocumentId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener documento con ID: {DocumentId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
        /// <summary>
        /// Crea un documento en el sistema.
        /// </summary>
        /// <param name="Document">Datos del documento a crear</param>
        /// <returns>Documento creado</returns>
        /// <response code="201">Retorna el documento creado</response>
        /// <response code="400">Datos del documento no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(DocumentDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateDocument([FromBody] DocumentDTO Document)
        {
            try
            {
                var createdDocument = await _DocumentBusiness.CreateDocumentAsync(Document);
                return CreatedAtAction(nameof(GetDocumentById), new { id = createdDocument.Id }, createdDocument);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear documento");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear documento");
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PatchDocument(int id, [FromBody] DocumentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _DocumentBusiness.PatchDocumentAsync(id, dto.Name, dto.Url);
                if (!updated)
                    return NotFound(new { message = "Documento no encontrado" });

                return Ok(new { message = "Documento actualizado correctamente", id = id });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new { message = "Documento no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PatchDocument");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }


        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutDocument(int id, [FromBody] DocumentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _DocumentBusiness.PutDocumentAsync(id, dto);
                if (!updated)
                    return NotFound(new { message = "Documento no encontrado" });

                return Ok(new { message = "Documento actualizado correctamente", id = id });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (EntityNotFoundException)
            {
                return NotFound(new { message = "Documento no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PutDocument");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }














    }
}