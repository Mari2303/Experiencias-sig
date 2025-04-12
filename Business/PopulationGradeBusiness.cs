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
        private readonly ILogger<PopulationGrade> _logger;

        public PopulationGradeBusiness(PopulationGradeData populationGradeData, ILogger<PopulationGrade> logger)
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
             
                return MapToDTOList(grades);
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


                return MapToDTO(grade);





            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado de población con ID: {GradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado de población con ID {id}", ex);
            }
        }

        // Método para crear un grado de población desde un DTO
        public async Task<PopulationGradeDTO> CreatePopulationGradeAsync(PopulationGradeDTO gradeDTO)
        {
            try
            {
                ValidatePopulationGrade(gradeDTO);

               var grade = MapToEntity(gradeDTO);

                var createdGrade = await _populationGradeData.CreateAsync(grade);

                return MapToDTO(createdGrade);






            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado de población: {GradeName}", gradeDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado de población", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePopulationGrade(PopulationGradeDTO gradeDTO)
        {
            if (gradeDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto grado de población no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(gradeDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado de población con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del grado de población es obligatorio");
            }
        }


        // Metodo para  mapear de entidad a DTO
         
        private PopulationGradeDTO MapToDTO(PopulationGrade grade)
        {
            return new PopulationGradeDTO
            {
                Id = grade.Id,
                Name = grade.Name
            };
        }

        // Metodo para mapear de DTO a entidad

        private PopulationGrade MapToEntity(PopulationGradeDTO gradeDTO)
        {
            return new PopulationGrade
            {
                Id = gradeDTO.Id,
                Name = gradeDTO.Name
            };
        }


        // Método para mapear una lista de entidades a DTOs

        private IEnumerable<PopulationGradeDTO> MapToDTOList(IEnumerable<PopulationGrade> grades)
        {
           var gradeDTOs = new List<PopulationGradeDTO>();
            foreach (var grade in grades)
            {
                gradeDTOs.Add(MapToDTO(grade));
            }
            return gradeDTOs;
        }


    }
}
