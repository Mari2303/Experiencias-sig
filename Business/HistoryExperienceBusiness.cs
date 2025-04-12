using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con el historial de experiencias en el sistema.
    /// </summary>
    public class HistoryExperienceBusiness
    {
        private readonly HistoryExperienceData _historyExperienceData;
        private readonly ILogger<HistoryExperience> _logger;

        public HistoryExperienceBusiness(HistoryExperienceData historyExperienceData, ILogger<HistoryExperience> logger)
        {
            _historyExperienceData = historyExperienceData;
            _logger = logger;
        }

        // Método para obtener todo el historial de experiencias como DTOs
        public async Task<IEnumerable<HistoryExperienceDTO>> GetAllHistoryExperiencesAsync()
        {
            try
            {
                var histories = await _historyExperienceData.GetAllAsync();
                

                return MapToDTOList(histories);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de experiencias");
                throw new ExternalServiceException("Base de datos", "Error al recuperar el historial de experiencias", ex);
            }
        }

        // Método para obtener un historial de experiencia por ID como DTO
        public async Task<HistoryExperienceDTO> GetHistoryExperienceByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un historial de experiencia con ID inválido: {HistoryId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del historial de experiencia debe ser mayor que cero");
            }

            try
            {
                var history = await _historyExperienceData.GetByIdAsync(id);
                if (history == null)
                {
                    _logger.LogInformation("No se encontró ningún historial de experiencia con ID: {HistoryId}", id);
                    throw new EntityNotFoundException("HistoryExperience", id);
                }



                return MapToDTO(history);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de experiencia con ID: {HistoryId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el historial de experiencia con ID {id}", ex);
            }
        }

        // Método para registrar una nueva entrada en el historial de experiencias
        public async Task<HistoryExperienceDTO> CreateHistoryExperienceAsync(HistoryExperienceDTO HistoryDTO)
        {
            try
            {



                ValidateHistoryExperience(HistoryDTO);

                var history = MapToEntity(HistoryDTO);

                var historyCreate = await _historyExperienceData.CreateAsync(history);

                return MapToDTO(historyCreate);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo : {HistoryExperienceNombre}", HistoryDTO?.TableName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear", ex);
            }
        }

     
        // Método para validar el DTO
        private void ValidateHistoryExperience(HistoryExperienceDTO HistoryExperienceDTO)
        {
            if (HistoryExperienceDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(HistoryExperienceDTO.TableName))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }


        // Método para mapear una lista de entidades a DTOs
        private HistoryExperienceDTO MapToDTO(HistoryExperience history)
        {
            return new HistoryExperienceDTO
            {
                Id = history.Id,
                DateTime = history.DateTime,
                UserId = history.UserId,
                TableName = history.TableName,
                ExperiencieId = history.ExperiencieId,
                Action = history.Action
            };
        }

        // Metodo para mapear de DTO a entidad

        private HistoryExperience MapToEntity(HistoryExperienceDTO historyDTO)
        {
            return new HistoryExperience
            {
                Id = historyDTO.Id,
                DateTime = historyDTO.DateTime,
                UserId = historyDTO.UserId,
                TableName = historyDTO.TableName,
                ExperiencieId = historyDTO.ExperiencieId,
                Action = historyDTO.Action
            };
        }



        // Método para mapear una lista de entidades a DTOs

        private IEnumerable<HistoryExperienceDTO> MapToDTOList(IEnumerable<HistoryExperience> histories)
        {
         var historyDTOs = new List<HistoryExperienceDTO>();
            foreach (var history in histories)
            {
                historyDTOs.Add(MapToDTO(history));
            }
            return historyDTOs;
        }

    }
}
