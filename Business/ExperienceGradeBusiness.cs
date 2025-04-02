using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los grados de experiencia en el sistema.
    /// </summary>
    public class ExperienceGradeBusiness
    {
        private readonly ExperienceGradeData _experienceGradeData;
        private readonly ILogger _logger;

        public ExperienceGradeBusiness(ExperienceGradeData experienceGradeData, ILogger logger)
        {
            _experienceGradeData = experienceGradeData;
            _logger = logger;
        }

        // Método para obtener todos los grados de experiencia como DTOs
        public async Task<IEnumerable<ExperienceGradeDto>> GetAllExperienceGradesAsync()
        {
            try
            {
                var experienceGrades = await _experienceGradeData.GetAllAsync();
                return experienceGrades.Select(experienceGrade => new ExperienceGradeDto
                {
                    Id = experienceGrade.Id,
                    Name = experienceGrade.Name,
                    Level = experienceGrade.Level,
                    CreatedAt = experienceGrade.CreatedAt,
                    UpdatedAt = experienceGrade.UpdatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los grados de experiencia");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de grados de experiencia", ex);
            }
        }

        // Método para obtener un grado de experiencia por ID como DTO
        public async Task<ExperienceGradeDto> GetExperienceGradeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un grado de experiencia con ID inválido: {ExperienceGradeId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del grado de experiencia debe ser mayor que cero");
            }

            try
            {
                var experienceGrade = await _experienceGradeData.GetByIdAsync(id);
                if (experienceGrade == null)
                {
                    _logger.LogInformation("No se encontró ningún grado de experiencia con ID: {ExperienceGradeId}", id);
                    throw new EntityNotFoundException("ExperienceGrade", id);
                }

                return new ExperienceGradeDto
                {
                    Id = experienceGrade.Id,
                    Name = experienceGrade.Name,
                    Level = experienceGrade.Level,
                    CreatedAt = experienceGrade.CreatedAt,
                    UpdatedAt = experienceGrade.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado de experiencia con ID: {ExperienceGradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado de experiencia con ID {id}", ex);
            }
        }

        // Método para crear un grado de experiencia desde un DTO
        public async Task<ExperienceGradeDto> CreateExperienceGradeAsync(ExperienceGradeDto experienceGradeDto)
        {
            try
            {
                ValidateExperienceGrade(experienceGradeDto);

                var experienceGrade = new ExperienceGrade
                {
                    Name = experienceGradeDto.Name,
                    Level = experienceGradeDto.Level,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdExperienceGrade = await _experienceGradeData.CreateAsync(experienceGrade);

                return new ExperienceGradeDto
                {
                    Id = createdExperienceGrade.Id,
                    Name = createdExperienceGrade.Name,
                    Level = createdExperienceGrade.Level,
                    CreatedAt = createdExperienceGrade.CreatedAt,
                    UpdatedAt = createdExperienceGrade.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado de experiencia: {ExperienceGradeName}", experienceGradeDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperienceGrade(ExperienceGradeDto experienceGradeDto)
        {
            if (experienceGradeDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto grado de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(experienceGradeDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado de experiencia con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del grado de experiencia es obligatorio");
            }

            if (experienceGradeDto.Level <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado de experiencia con Level inválido");
                throw new Utilities.Exceptions.ValidationException("Level", "El Level del grado de experiencia debe ser mayor que cero");
            }
        }
    }
}
