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
                

                return MapToDTOList(experienceLineThematics);


           
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

              
                return MapToDTO(lineThematic);





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

                var lineThematic = MapToEntity(ExperiencieLineThematicDTO);

                var lineThematicCreate = await _ExperiencieLineThematicData.CreateAsync(rol);

                return MapToDTO(lineThematicCreate);






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


        // Método para mapear una entidad a un DTO


        private ExperiencieLineThematicDTO MapToDTO(ExperiencieLineThematic lineThematic)
        {
            return new ExperiencieLineThematicDTO
            {
                Id = lineThematic.Id,
                LineThematicId = lineThematic.LineThematicId,
                ExperiencieId = lineThematic.ExperiencieId,
                
            };
        }

        // Metodo para mapear de DTO a entidad

        private ExperiencieLineThematic MapToEntity(ExperiencieLineThematicDTO lineThematicDTO)
        {
            return new ExperiencieLineThematic
            {
                Id = lineThematicDTO.Id,
                LineThematicId = lineThematicDTO.LineThematicId,
                ExperiencieId = lineThematicDTO.ExperiencieId,

            };
        }

        // Método para mapear una lista de entidades a una lista de DTOs

        private IEnumerable<ExperiencieLineThematicDTO> MapToDTOList(IEnumerable<ExperiencieLineThematic> lineThematics)
        {
          var lineThematicDTOs = new List<ExperiencieLineThematicDTO>();
            foreach (var lineThematic in lineThematics)
            {
                lineThematicDTOs.Add(MapToDTO(lineThematic));
            }
            return lineThematicDTOs;



        }





    }
}
