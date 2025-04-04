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
        private readonly ILogger _logger;

        public HistoryExperienceBusiness(HistoryExperienceData historyExperienceData, ILogger logger)
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
                return histories.Select(history => new HistoryExperienceDTO
                {
                    Id = history.Id,
                    DateTime = history.DateTime,
                    UserId = history.UserId,
                    TableName = history.TableName,
                    ExperienceId = history.ExperiencieId,
                    Action = history.Action
                });
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de experiencia con ID: {HistoryId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el historial de experiencia con ID {id}", ex);
            }
        }

        // Método para registrar una nueva entrada en el historial de experiencias
        public async Task<HistoryExperienceDTO> CreateHistoryExperienceAsync(HistoryExperienceDTO historyDto)
        {
            try
            {
                ValidateHistoryExperience(historyDto);

                var history = new HistoryExperience
                {
                 
                    DateTime = historyDto.DateTime,
                    UserId = historyDto.UserId,
                    TableName = historyDto.TableName,
                    ExperiencieId = historyDto.ExperiencieId,
                    Action = historyDto.Action
                    
                };

                var createdHistory = await _historyExperienceData.CreateAsync(history);

                return new HistoryExperienceDTO
                {
                    Id = createdHistory.Id,
                    DateTime = createdHistory.DateTime,
                    UserId = createdHistory.UserId,
                    TableName = createdHistory.TableName,
                    ExperiencieId = createdHistory.ExperiencieId,
                    Action = createdHistory.Action
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo : {HistoryExperienceNombre}", historyDto?.TableName ?? "null");
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
    }
}
