using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los objetivos en el sistema.
    /// </summary>
    public class ObjectiveBusiness
    {
        private readonly ObjectiveData _objectiveData;
        private readonly ILogger _logger;

        public ObjectiveBusiness(ObjectiveData objectiveData, ILogger logger)
        {
            _objectiveData = objectiveData;
            _logger = logger;
        }

        // Método para obtener todos los objetivos como DTOs
        public async Task<IEnumerable<ObjectiveDto>> GetAllObjectivesAsync()
        {
            try
            {
                var objectives = await _objectiveData.GetAllAsync();
                var objectiveDTOs = objectives.Select(obj => new ObjectiveDto
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Description = obj.Description,
                    CreatedAt = obj.CreatedAt,
                    UpdatedAt = obj.UpdatedAt
                }).ToList();

                return objectiveDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los objetivos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de objetivos", ex);
            }
        }

        // Método para obtener un objetivo por ID como DTO
        public async Task<ObjectiveDto> GetObjectiveByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un objetivo con ID inválido: {ObjectiveId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del objetivo debe ser mayor que cero");
            }

            try
            {
                var objective = await _objectiveData.GetByIdAsync(id);
                if (objective == null)
                {
                    _logger.LogInformation("No se encontró ningún objetivo con ID: {ObjectiveId}", id);
                    throw new EntityNotFoundException("Objective", id);
                }

                return new ObjectiveDto
                {
                    Id = objective.Id,
                    Name = objective.Name,
                    Description = objective.Description,
                    CreatedAt = objective.CreatedAt,
                    UpdatedAt = objective.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el objetivo con ID: {ObjectiveId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el objetivo con ID {id}", ex);
            }
        }

        // Método para crear un objetivo desde un DTO
        public async Task<ObjectiveDto> CreateObjectiveAsync(ObjectiveDto objectiveDto)
        {
            try
            {
                ValidateObjective(objectiveDto);

                var objective = new Objective
                {
                    Name = objectiveDto.Name,
                    Description = objectiveDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdObjective = await _objectiveData.CreateAsync(objective);

                return new ObjectiveDto
                {
                    Id = createdObjective.Id,
                    Name = createdObjective.Name,
                    Description = createdObjective.Description,
                    CreatedAt = createdObjective.CreatedAt,
                    UpdatedAt = createdObjective.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo objetivo: {ObjectiveName}", objectiveDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el objetivo", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateObjective(ObjectiveDto objectiveDto)
        {
            if (objectiveDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto objetivo no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(objectiveDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un objetivo con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del objetivo es obligatorio");
            }
        }
    }
}
