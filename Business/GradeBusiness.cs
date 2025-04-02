using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los grados en el sistema.
    /// </summary>
    public class GradeBusiness
    {
        private readonly GradeData _gradeData;
        private readonly ILogger _logger;

        public GradeBusiness(GradeData gradeData, ILogger logger)
        {
            _gradeData = gradeData;
            _logger = logger;
        }

        // Método para obtener todos los grados como DTOs
        public async Task<IEnumerable<GradeDto>> GetAllGradesAsync()
        {
            try
            {
                var grades = await _gradeData.GetAllAsync();
                return grades.Select(grade => new GradeDto
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    Level = grade.Level,
                    CreatedAt = grade.CreatedAt,
                    UpdatedAt = grade.UpdatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los grados");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de grados", ex);
            }
        }

        // Método para obtener un grado por ID como DTO
        public async Task<GradeDto> GetGradeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un grado con ID inválido: {GradeId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del grado debe ser mayor que cero");
            }

            try
            {
                var grade = await _gradeData.GetByIdAsync(id);
                if (grade == null)
                {
                    _logger.LogInformation("No se encontró ningún grado con ID: {GradeId}", id);
                    throw new EntityNotFoundException("Grade", id);
                }

                return new GradeDto
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    Level = grade.Level,
                    CreatedAt = grade.CreatedAt,
                    UpdatedAt = grade.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado con ID: {GradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado con ID {id}", ex);
            }
        }

        // Método para crear un grado desde un DTO
        public async Task<GradeDto> CreateGradeAsync(GradeDto gradeDto)
        {
            try
            {
                ValidateGrade(gradeDto);

                var grade = new Grade
                {
                    Name = gradeDto.Name,
                    Level = gradeDto.Level,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdGrade = await _gradeData.CreateAsync(grade);

                return new GradeDto
                {
                    Id = createdGrade.Id,
                    Name = createdGrade.Name,
                    Level = createdGrade.Level,
                    CreatedAt = createdGrade.CreatedAt,
                    UpdatedAt = createdGrade.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado: {GradeName}", gradeDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateGrade(GradeDto gradeDto)
        {
            if (gradeDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto grado no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(gradeDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del grado es obligatorio");
            }

            if (gradeDto.Level <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado con Level inválido");
                throw new Utilities.Exceptions.ValidationException("Level", "El Level del grado debe ser mayor que cero");
            }
        }
    }
}