using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;
using ValidationException = Utilities.Exeptions.ValidationException;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las líneas temáticas de experiencias en el sistema.
    /// </summary>
    public class ExperiencieLineThematicBusiness
    {
        private readonly ExperiencieLineThematicData _ExperiencieLineThematicData;
        private readonly ILogger<ExperiencieLineThematic> _logger;

        public ExperiencieLineThematicBusiness(ExperiencieLineThematicData ExperiencieLineThematicData, ILogger<ExperiencieLineThematic> logger)
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

                var lineThematicCreate = await _ExperiencieLineThematicData.CreateAsync(lineThematic);

                return MapToDTO(lineThematicCreate);  






            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva línea temática de experiencia: {ExperienceLineThematicName}", ExperiencieLineThematicDTO?.LineThematicId);
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

            if (ExperiencieLineThematicDTO.LineThematicId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar una línea temática de LineThematicId con Name vacío: {LineThematicId}", ExperiencieLineThematicDTO.LineThematicId);
                throw new Utilities.Exeptions.ValidationException("Name", "El Name de la línea temática de experiencia es obligatorio");
            }
        }



        public async Task<bool> PatchExperiencieLineThematicAsync(int id, int experiencieId, int lineThematicId)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (experiencieId <= 0)
                throw new ValidationException("experiencieId", "El ID de la experiencia es obligatorio.");

            if (lineThematicId <= 0)
                throw new ValidationException("lineThematicId", "El ID de la línea temática es obligatorio.");

            var result = await _ExperiencieLineThematicData.PatchExperiencieLineThematicAsync(id, experiencieId, lineThematicId);

            if (!result)
                throw new EntityNotFoundException("ExperiencieLineThematic", id);

            return true;
        }



        public async Task<bool> PutExperiencieLineThematicAsync(int id, int experiencieId, int lineThematicId)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (experiencieId <= 0)
                throw new ValidationException("experiencieId", "El ID de la experiencia es obligatorio.");

            if (lineThematicId <= 0)
                throw new ValidationException("lineThematicId", "El ID de la línea temática es obligatorio.");

            var result = await _ExperiencieLineThematicData.PutExperiencieLineThematicAsync(id, experiencieId, lineThematicId);

            if (!result)
                throw new EntityNotFoundException("ExperiencieLineThematic", id);

            return true;
        }







        // Método para mapear una entidad a un DTO


        private ExperiencieLineThematicDTO MapToDTO(ExperiencieLineThematic lineThematic)
        {
            return new ExperiencieLineThematicDTO
            {
                Id = lineThematic.Id,
                LineThematicId = lineThematic.LineThematicId,
                LineThematicName = lineThematic.LineThematicName,
                ExperiencieId = lineThematic.ExperiencieId,
                ExperiencieName = lineThematic.ExperiencieName

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
