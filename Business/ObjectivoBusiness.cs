using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Utilities.Exeptions;

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
        public async Task<IEnumerable<ObjectiveDTO>> GetAllObjectivesAsync()
        {
            try
            {
                var objectives = await _objectiveData.GetAllAsync();


                return MapToDTOList(objectives);





            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los objetivos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de objetivos", ex);
            }
        }

        // Método para obtener un objetivo por ID como DTO
        public async Task<ObjectiveDTO> GetObjectiveByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un objetivo con ID inválido: {ObjectiveId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del objetivo debe ser mayor que cero");
            }

            try
            {
                var objective = await _objectiveData.GetByIdAsync(id);
                if (objective == null)
                {
                    _logger.LogInformation("No se encontró ningún objetivo con ID: {ObjectiveId}", id);
                    throw new EntityNotFoundException("Objective", id);
                }


                return MapToDTO(objective);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el objetivo con ID: {ObjectiveId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el objetivo con ID {id}", ex);
            }
        }

        // Método para crear un objetivo desde un DTO
        public async Task<ObjectiveDTO> CreateObjectiveAsync(ObjectiveDTO ObjectiveDTO)
        {
            try
            {
                ValidateObjective(ObjectiveDTO);

                var objective = MapToEntity(ObjectiveDTO);

                var objectiveCreate = await _objectiveData.CreateAsync(objective);

                return MapToDTO(objectiveCreate);








            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo objetivo: {ObjectiveName}", ObjectiveDTO?.ObjetiveDescription ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el objetivo", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateObjective(ObjectiveDTO ObjectiveDTO)
        {
            if (ObjectiveDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto objetivo no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(ObjectiveDTO.ObjetiveDescription))
            {
                _logger.LogWarning("Se intentó crear/actualizar un objetivo con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del objetivo es obligatorio");
            }
        }
    
        // Metodo para mapear de entidad a DTO

        private ObjectiveDTO MapToDTO(Objective objective)
        {
            return new ObjectiveDTO

            {
                Id = objective.Id,
                ObjetiveDescription = objective.ObjectiveDescription,
                Innovation = objective.Innovation,
                Results = objective.Results,
                Sustainability = objective.Sustainability,
                ExperienceId = objective.ExperienceId
            };
        }


        // Método para mapear de DTO a entidad


        private Objective MapToEntity(ObjectiveDTO objectiveDTO)
        {
            return new Objective
            {
                Id = objectiveDTO.Id,
                ObjectiveDescription = objectiveDTO.ObjetiveDescription,
                Innovation = objectiveDTO.Innovation,
                Results = objectiveDTO.Results,
                Sustainability = objectiveDTO.Sustainability,
                ExperienceId = objectiveDTO.ExperienceId
            };
        }
        // Método para mapear una lista de entidades a DTOs

        private IEnumerable<ObjectiveDTO> MapToDTOList(IEnumerable<Objective> objectives)
        {
          var objectiveDTOs = new List<ObjectiveDTO>();
            foreach (var objective in objectives)
            {
                objectiveDTOs.Add(MapToDTO(objective));
            }
            return objectiveDTOs;
        }

    }
}
