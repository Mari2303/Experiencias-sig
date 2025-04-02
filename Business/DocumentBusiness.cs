using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los documentos del sistema.
    /// </summary>
    public class DocumentBusiness
    {
        private readonly DocumentData _documentData;
        private readonly ILogger _logger;

        public DocumentBusiness(DocumentData documentData, ILogger logger)
        {
            _documentData = documentData;
            _logger = logger;
        }

        // Método para obtener todos los documentos como DTOs
        public async Task<IEnumerable<DocumentDTO>> GetAllDocumentsAsync()
        {
            try
            {
                var documents = await _documentData.GetAllAsync();
                var DocumentDTOs = new List<DocumentDTO>();

                foreach (var document in documents)
                {
                    DocumentDTOs.Add(new DocumentDTO
                    {
                        Id = document.Id,
                        Name = document.Name,
                        Url = document.Url,
                       
                    });
                }

                return DocumentDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los documentos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de documentos", ex);
            }
        }

        // Método para obtener un documento por ID como DTO
        public async Task<DocumentDTO> GetDocumentByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un documento con ID inválido: {DocumentId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del documento debe ser mayor que cero");
            }

            try
            {
                var document = await _documentData.GetByIdAsync(id);
                if (document == null)
                {
                    _logger.LogInformation("No se encontró ningún documento con ID: {DocumentId}", id);
                    throw new EntityNotFoundException("Document", id);
                }

                return new DocumentDTO
                {
                    Id = document.Id,
                    Name = document.Name,
                    Url = document.Url,

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el documento con ID: {DocumentId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el documento con ID {id}", ex);
            }
        }

        // Método para crear un documento desde un DTO
        public async Task<DocumentDTO> CreateDocumentAsync(DocumentDTO DocumentDTO)
        {
            try
            {
                ValidateDocument(DocumentDTO);

                var document = new Document
                {
                    Name = DocumentDTO.Name,
                    Url= DocumentDTO.Url,
                   
                };

                var documentCreated = await _documentData.CreateAsync(document);

                return new DocumentDTO
                {
                   
                    Id = documentCreated.Id,
                    Name = documentCreated.Name,
                    Url = documentCreated.Url,

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo documento: {DocumentTitle}", DocumentDTO?.Name?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el documento", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateDocument(DocumentDTO DocumentDTO)
        {
            if (DocumentDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto documento no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(DocumentDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un documento con Title vacío");
                throw new Utilities.Exeptions.ValidationException("Title", "El Title del documento es obligatorio");
            }
        }
    }
}
