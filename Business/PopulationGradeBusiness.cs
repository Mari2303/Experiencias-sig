using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los grados de población en el sistema.
    /// </summary>
    public class PopulationGradeBusiness
    {
        private readonly PopulationGradeData _populationGradeData;
        private readonly ILogger _logger;

        public PopulationGradeBusiness(PopulationGradeData populationGradeData, ILogger logger)
        {
            _populationGradeData = populationGradeData;
            _logger = logger;
        }

        // Método para obtener todos los grados de población como DTOs
        public async Task<IEnumerable<PopulationGradeDto>> GetAllPopulationGradesAsync()
        {
            try
            {
                var grades = await _populationGradeData.GetAllAsync();
                return grades.Select(grade => new PopulationGradeDto
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    Description = grade.Description,
                    CreatedAt = grade.CreatedAt,
                    UpdatedAt = grade.UpdatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los grados de población");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de grados de población", ex);
            }
        }

        // Método para obtener un grado de población por ID como DTO
        public async Task<PopulationGradeDto> GetPopulationGradeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un grado de población con ID inválido: {GradeId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del grado de población debe ser mayor que cero");
            }

            try
            {
                var grade = await _populationGradeData.GetByIdAsync(id);
                if (grade == null)
                {
                    _logger.LogInformation("No se encontró ningún grado de población con ID: {GradeId}", id);
                    throw new EntityNotFoundException("PopulationGrade", id);
                }

                return new PopulationGradeDto
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    Description = grade.Description,
                    CreatedAt = grade.CreatedAt,
                    UpdatedAt = grade.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado de población con ID: {GradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado de población con ID {id}", ex);
            }
        }

        // Método para crear un grado de población desde un DTO
        public async Task<PopulationGradeDto> CreatePopulationGradeAsync(PopulationGradeDto gradeDto)
        {
            try
            {
                ValidatePopulationGrade(gradeDto);

                var grade = new PopulationGrade
                {
                    Name = gradeDto.Name,
                    Description = gradeDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdGrade = await _populationGradeData.CreateAsync(grade);

                return new PopulationGradeDto
                {
                    Id = createdGrade.Id,
                    Name = createdGrade.Name,
                    Description = createdGrade.Description,
                    CreatedAt = createdGrade.CreatedAt,
                    UpdatedAt = createdGrade.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado de población: {GradeName}", gradeDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado de población", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePopulationGrade(PopulationGradeDto gradeDto)
        {
            if (gradeDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto grado de población no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(gradeDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado de población con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del grado de población es obligatorio");
            }
        }
    }
}
