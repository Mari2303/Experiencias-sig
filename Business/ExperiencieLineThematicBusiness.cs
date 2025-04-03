using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las líneas temáticas de experiencias en el sistema.
    /// </summary>
    public class ExperiencieLineThematicBusiness
    {
        private readonly ExperiencieLineThematicData _ExperiencieLineThematicData;
        private readonly ILogger _logger;

        public ExperiencieLineThematicBusiness(ExperiencieLineThematicData ExperiencieLineThematicData, ILogger logger)
        {
            _ExperiencieLineThematicData = ExperiencieLineThematicData;
            _logger = logger;
        }

        // Método para obtener todas las líneas temáticas de experiencias como DTOs
        public async Task<IEnumerable<ExperiencieLineThematicDTO>> GetAllExperienceLineThematicsAsync()
        {
            try
            {
                var experienceLineThematics = await _ExperiencieLineThematicData.GetAllAsync();
                var ExperiencieLineThematicDTOs = new List<ExperiencieLineThematicDTO>();

                foreach (var lineThematic in experienceLineThematics)
                {
                    ExperiencieLineThematicDTOs.Add(new ExperiencieLineThematicDTO
                    {
                        Id = lineThematic.Id,
                       LineThematicId = lineThematic.LineThematicId,
                       ExperiencieId = lineThematic.ExperiencieId

                    });
                }

                return ExperiencieLineThematicDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las líneas temáticas de experiencias");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de líneas temáticas de experiencias", ex);
            }
        }

        // Método para obtener una línea temática de experiencia por ID como DTO
        public async Task<ExperiencieLineThematicDTO> GetExperienceLineThematicByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una línea temática de experiencia con ID inválido: {ExperienceLineThematicId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID de la línea temática de experiencia debe ser mayor que cero");
            }

            try
            {
                var lineThematic = await _ExperiencieLineThematicData.GetByIdAsync(id);
                if (lineThematic == null)
                {
                    _logger.LogInformation("No se encontró ninguna línea temática de experiencia con ID: {ExperienceLineThematicId}", id);
                    throw new EntityNotFoundException("ExperienceLineThematic", id);
                }

                return new ExperiencieLineThematicDTO
                {
                    Id = lineThematic.Id,
                    LineThematicId = lineThematic.LineThematicId,
                    ExperiencieId = lineThematic.ExperiencieId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la línea temática de experiencia con ID: {ExperienceLineThematicId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la línea temática de experiencia con ID {id}", ex);
            }
        }

        // Método para crear una línea temática de experiencia desde un DTO
        public async Task<ExperiencieLineThematicDTO> CreateExperienceLineThematicAsync(ExperiencieLineThematicDTO ExperiencieLineThematicDTO)
        {
            try
            {
                ValidateExperienceLineThematic(ExperiencieLineThematicDTO);

                var lineThematic = new ExperiencieLineThematic
                {
                    LineThematicId = ExperiencieLineThematicDTO.LineThematicId,
                    ExperiencieId = ExperiencieLineThematicDTO.ExperiencieId
                 
                };

                var lineThematicCreated = await _ExperiencieLineThematicData.CreateAsync(lineThematic);

                return new ExperiencieLineThematicDTO
                {

                    Id = lineThematicCreated.Id,
                    LineThematicId = lineThematicCreated.LineThematicId,
                    ExperiencieId = lineThematicCreated.ExperiencieId

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva línea temática de experiencia: {ExperienceLineThematicName}", ExperiencieLineThematicDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la línea temática de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperienceLineThematic(ExperiencieLineThematicDTO ExperiencieLineThematicDTO)
        {
            if (ExperiencieLineThematicDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto línea temática de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(ExperiencieLineThematicDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una línea temática de experiencia con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name de la línea temática de experiencia es obligatorio");
            }
        }
    }
}
