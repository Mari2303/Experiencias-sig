using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;

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
        public async Task<IEnumerable<PopulationGradeDTO>> GetAllPopulationGradesAsync()
        {
            try
            {
                var grades = await _populationGradeData.GetAllAsync();
                var gradesDTO = new List<PopulationGradeDTO>();

                foreach (var grade in grades)
                {
                    gradesDTO.Add(new PopulationGradeDTO

                    {

                        Id = grade.Id,
                        Name = grade.Name

                    });
                }


                return gradesDTO;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los grados de población");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de grados de población", ex);
            }
        }

        // Método para obtener un grado de población por ID como DTO
        public async Task<PopulationGradeDTO> GetPopulationGradeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un grado de población con ID inválido: {GradeId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del grado de población debe ser mayor que cero");
            }

            try
            {
                var grade = await _populationGradeData.GetByIdAsync(id);
                if (grade == null)
                {
                    _logger.LogInformation("No se encontró ningún grado de población con ID: {GradeId}", id);
                    throw new EntityNotFoundException("PopulationGrade", id);
                }

                return new PopulationGradeDTO
                {
                    Id = grade.Id,
                    Name = grade.Name,
                   
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado de población con ID: {GradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado de población con ID {id}", ex);
            }
        }

        // Método para crear un grado de población desde un DTO
        public async Task<PopulationGradeDTO> CreatePopulationGradeAsync(PopulationGradeDTO gradeDto)
        {
            try
            {
                ValidatePopulationGrade(gradeDto);

                var grade = new PopulationGrade
                {
                    Name = gradeDto.Name,
                    
                };

                var createdGrade = await _populationGradeData.CreateAsync(grade);

                return new PopulationGradeDTO
                {
                    Id = createdGrade.Id,
                    Name = createdGrade.Name,
                   
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado de población: {GradeName}", gradeDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado de población", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePopulationGrade(PopulationGradeDTO gradeDto)
        {
            if (gradeDto == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto grado de población no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(gradeDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado de población con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del grado de población es obligatorio");
            }
        }
    }
}
