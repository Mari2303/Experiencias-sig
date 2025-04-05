using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los grados de experiencia en el sistema.
    /// </summary>
    public class ExperienceGradeBusiness
    {
        private readonly ExperiencieGradeData _experienceGradeData;
        private readonly ILogger _logger;

        public ExperienceGradeBusiness(ExperiencieGradeData experienceGradeData, ILogger logger)
        {
            _experienceGradeData = experienceGradeData;
            _logger = logger;
        }

        // Método para obtener todos los grados de experiencia como DTOs
        public async Task<IEnumerable<ExperiencieGradeDTO>> GetAllExperienceGradesAsync()
        {
            try
            {

                var experienceGrades = await _experienceGradeData.GetAllAsync();

                return MapToDTOList(experienceGrades);

                return experienceGradesDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los grados de experiencia");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de grados de experiencia", ex);
            }
        }

        // Método para obtener un grado de experiencia por ID como DTO
        public async Task<ExperiencieGradeDTO> GetExperienceGradeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un grado de experiencia con ID inválido: {ExperienceGradeId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del grado de experiencia debe ser mayor que cero");
            }

            try
            {
                var experienceGrade = await _experienceGradeData.GetByIdAsync(id);
                if (experienceGrade == null)
                {
                    _logger.LogInformation("No se encontró ningún grado de experiencia con ID: {ExperienceGradeId}", id);
                    throw new EntityNotFoundException("ExperienceGrade", id);
                }

                return new ExperiencieGradeDTO
                {
                    Id = experienceGrade.Id,
                    GradeId = experienceGrade.GradeId,
                    ExperiencieId = experienceGrade.ExperiencieId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado de experiencia con ID: {ExperienceGradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado de experiencia con ID {id}", ex);
            }
        }

        // Método para crear un grado de experiencia desde un DTO
        public async Task<ExperiencieGradeDTO> CreateExperiencieGradeAsync(ExperiencieGradeDTO ExperiencieGradeDTO)
        {
            try
            {
                ValidateExperienceGrade(ExperiencieGradeDTO);

                var experiencieGrade = new ExperiencieGrade
                {
                    
                    GradeId = ExperiencieGradeDTO.GradeId,
                    ExperiencieId = ExperiencieGradeDTO.ExperiencieId
                    
                };

                var createdExperiencieGrade = await  _experienceGradeData.CreateAsync(experiencieGrade);
                return new ExperiencieGradeDTO
                {
                    Id = createdExperiencieGrade.Id,
                    GradeId = createdExperiencieGrade.GradeId,
                    ExperiencieId = createdExperiencieGrade.ExperiencieId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado de experiencia: {ExperienceGradeName}", ExperiencieGradeDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperienceGrade(ExperiencieGradeDTO ExperiencieGradeDTO)
        {
            if (ExperiencieGradeDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto grado de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(ExperiencieGradeDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado de experiencia con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del grado de experiencia es obligatorio");
            }
            
        }
    }
}
