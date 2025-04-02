using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

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
        public async Task<IEnumerable<HistoryExperienceDto>> GetAllHistoryExperiencesAsync()
        {
            try
            {
                var histories = await _historyExperienceData.GetAllAsync();
                return histories.Select(history => new HistoryExperienceDto
                {
                    Id = history.Id,
                    ExperienceId = history.ExperienceId,
                    UserId = history.UserId,
                    Action = history.Action,
                    Timestamp = history.Timestamp
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de experiencias");
                throw new ExternalServiceException("Base de datos", "Error al recuperar el historial de experiencias", ex);
            }
        }

        // Método para obtener un historial de experiencia por ID como DTO
        public async Task<HistoryExperienceDto> GetHistoryExperienceByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un historial de experiencia con ID inválido: {HistoryId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del historial de experiencia debe ser mayor que cero");
            }

            try
            {
                var history = await _historyExperienceData.GetByIdAsync(id);
                if (history == null)
                {
                    _logger.LogInformation("No se encontró ningún historial de experiencia con ID: {HistoryId}", id);
                    throw new EntityNotFoundException("HistoryExperience", id);
                }

                return new HistoryExperienceDto
                {
                    Id = history.Id,
                    ExperienceId = history.ExperienceId,
                    UserId = history.UserId,
                    Action = history.Action,
                    Timestamp = history.Timestamp
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de experiencia con ID: {HistoryId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el historial de experiencia con ID {id}", ex);
            }
        }

        // Método para registrar una nueva entrada en el historial de experiencias
        public async Task<HistoryExperienceDto> CreateHistoryExperienceAsync(HistoryExperienceDto historyDto)
        {
            try
            {
                ValidateHistoryExperience(historyDto);

                var history = new HistoryExperience
                {
                    ExperienceId = historyDto.ExperienceId,
                    UserId = historyDto.UserId,
                    Action = historyDto.Action,
                    Timestamp = DateTime.UtcNow
                };

                var createdHistory = await _historyExperienceData.CreateAsync(history);

                return new HistoryExperienceDto
                {
                    Id = createdHistory.Id,
                    ExperienceId = createdHistory.ExperienceId,
                    UserId = createdHistory.UserId,
                    Action = createdHistory.Action,
                    Timestamp = createdHistory.Timestamp
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar una nueva entrada en el historial de experiencias");
                throw new ExternalServiceException("Base de datos", "Error al registrar el historial de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateHistoryExperience(HistoryExperienceDto historyDto)
        {
            if (historyDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto historial de experiencia no puede ser nulo");
            }

            if (historyDto.ExperienceId <= 0)
            {
                _logger.LogWarning("Se intentó registrar un historial de experiencia con ExperienceId inválido");
                throw new Utilities.Exceptions.ValidationException("ExperienceId", "El ExperienceId debe ser mayor que cero");
            }

            if (historyDto.UserId <= 0)
            {
                _logger.LogWarning("Se intentó registrar un historial de experiencia con UserId inválido");
                throw new Utilities.Exceptions.ValidationException("UserId", "El UserId debe ser mayor que cero");
            }
        }
    }
}
