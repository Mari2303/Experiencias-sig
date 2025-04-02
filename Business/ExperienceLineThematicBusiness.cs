using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las líneas temáticas de experiencias en el sistema.
    /// </summary>
    public class ExperienceLineThematicBusiness
    {
        private readonly ExperienceLineThematicData _experienceLineThematicData;
        private readonly ILogger _logger;

        public ExperienceLineThematicBusiness(ExperienceLineThematicData experienceLineThematicData, ILogger logger)
        {
            _experienceLineThematicData = experienceLineThematicData;
            _logger = logger;
        }

        // Método para obtener todas las líneas temáticas de experiencias como DTOs
        public async Task<IEnumerable<ExperienceLineThematicDto>> GetAllExperienceLineThematicsAsync()
        {
            try
            {
                var experienceLineThematics = await _experienceLineThematicData.GetAllAsync();
                var experienceLineThematicDTOs = new List<ExperienceLineThematicDto>();

                foreach (var lineThematic in experienceLineThematics)
                {
                    experienceLineThematicDTOs.Add(new ExperienceLineThematicDto
                    {
                        Id = lineThematic.Id,
                        Name = lineThematic.Name,
                        Description = lineThematic.Description,
                        CreatedAt = lineThematic.CreatedAt,
                        UpdatedAt = lineThematic.UpdatedAt
                    });
                }

                return experienceLineThematicDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las líneas temáticas de experiencias");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de líneas temáticas de experiencias", ex);
            }
        }

        // Método para obtener una línea temática de experiencia por ID como DTO
        public async Task<ExperienceLineThematicDto> GetExperienceLineThematicByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una línea temática de experiencia con ID inválido: {ExperienceLineThematicId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la línea temática de experiencia debe ser mayor que cero");
            }

            try
            {
                var lineThematic = await _experienceLineThematicData.GetByIdAsync(id);
                if (lineThematic == null)
                {
                    _logger.LogInformation("No se encontró ninguna línea temática de experiencia con ID: {ExperienceLineThematicId}", id);
                    throw new EntityNotFoundException("ExperienceLineThematic", id);
                }

                return new ExperienceLineThematicDto
                {
                    Id = lineThematic.Id,
                    Name = lineThematic.Name,
                    Description = lineThematic.Description,
                    CreatedAt = lineThematic.CreatedAt,
                    UpdatedAt = lineThematic.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la línea temática de experiencia con ID: {ExperienceLineThematicId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la línea temática de experiencia con ID {id}", ex);
            }
        }

        // Método para crear una línea temática de experiencia desde un DTO
        public async Task<ExperienceLineThematicDto> CreateExperienceLineThematicAsync(ExperienceLineThematicDto experienceLineThematicDto)
        {
            try
            {
                ValidateExperienceLineThematic(experienceLineThematicDto);

                var lineThematic = new ExperienceLineThematic
                {
                    Name = experienceLineThematicDto.Name,
                    Description = experienceLineThematicDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var lineThematicCreated = await _experienceLineThematicData.CreateAsync(lineThematic);

                return new ExperienceLineThematicDto
                {
                    Id = lineThematicCreated.Id,
                    Name = lineThematicCreated.Name,
                    Description = lineThematicCreated.Description,
                    CreatedAt = lineThematicCreated.CreatedAt,
                    UpdatedAt = lineThematicCreated.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva línea temática de experiencia: {ExperienceLineThematicName}", experienceLineThematicDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la línea temática de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperienceLineThematic(ExperienceLineThematicDto experienceLineThematicDto)
        {
            if (experienceLineThematicDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto línea temática de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(experienceLineThematicDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una línea temática de experiencia con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la línea temática de experiencia es obligatorio");
            }
        }
    }
}
