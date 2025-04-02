using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las experiencias en el sistema.
    /// </summary>
    public class ExperienceBusiness
    {
        private readonly ExperienceData _experienceData;
        private readonly ILogger _logger;

        public ExperienceBusiness(ExperienceData experienceData, ILogger logger)
        {
            _experienceData = experienceData;
            _logger = logger;
        }

        // Método para obtener todas las experiencias como DTOs
        public async Task<IEnumerable<ExperienceDto>> GetAllExperiencesAsync()
        {
            try
            {
                var experiences = await _experienceData.GetAllAsync();
                var experienceDTOs = new List<ExperienceDto>();

                foreach (var experience in experiences)
                {
                    experienceDTOs.Add(new ExperienceDto
                    {
                        Id = experience.Id,
                        Title = experience.Title,
                        Description = experience.Description,
                        Category = experience.Category,
                        CreatedAt = experience.CreatedAt,
                        UpdatedAt = experience.UpdatedAt,
                        Rating = experience.Rating
                    });
                }

                return experienceDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las experiencias");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de experiencias", ex);
            }
        }

        // Método para obtener una experiencia por ID como DTO
        public async Task<ExperienceDto> GetExperienceByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una experiencia con ID inválido: {ExperienceId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la experiencia debe ser mayor que cero");
            }

            try
            {
                var experience = await _experienceData.GetByIdAsync(id);
                if (experience == null)
                {
                    _logger.LogInformation("No se encontró ninguna experiencia con ID: {ExperienceId}", id);
                    throw new EntityNotFoundException("Experience", id);
                }

                return new ExperienceDto
                {
                    Id = experience.Id,
                    Title = experience.Title,
                    Description = experience.Description,
                    Category = experience.Category,
                    CreatedAt = experience.CreatedAt,
                    UpdatedAt = experience.UpdatedAt,
                    Rating = experience.Rating
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la experiencia con ID: {ExperienceId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la experiencia con ID {id}", ex);
            }
        }

        // Método para crear una experiencia desde un DTO
        public async Task<ExperienceDto> CreateExperienceAsync(ExperienceDto experienceDto)
        {
            try
            {
                ValidateExperience(experienceDto);

                var experience = new Experience
                {
                    Title = experienceDto.Title,
                    Description = experienceDto.Description,
                    Category = experienceDto.Category,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Rating = experienceDto.Rating
                };

                var experienceCreated = await _experienceData.CreateAsync(experience);

                return new ExperienceDto
                {
                    Id = experienceCreated.Id,
                    Title = experienceCreated.Title,
                    Description = experienceCreated.Description,
                    Category = experienceCreated.Category,
                    CreatedAt = experienceCreated.CreatedAt,
                    UpdatedAt = experienceCreated.UpdatedAt,
                    Rating = experienceCreated.Rating
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva experiencia: {ExperienceTitle}", experienceDto?.Title ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperience(ExperienceDto experienceDto)
        {
            if (experienceDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(experienceDto.Title))
            {
                _logger.LogWarning("Se intentó crear/actualizar una experiencia con Title vacío");
                throw new Utilities.Exceptions.ValidationException("Title", "El Title de la experiencia es obligatorio");
            }
        }
    }
}
