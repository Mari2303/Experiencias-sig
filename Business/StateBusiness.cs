
using Microsoft.Extensions.Logging;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los estados en el sistema.
    /// </summary>
    public class StateBusiness
    {
        private readonly StateData _stateData;
        private readonly ILogger _logger;

        public StateBusiness(StateData stateData, ILogger logger)
        {
            _stateData = stateData;
            _logger = logger;
        }

        // Método para obtener todos los estados como DTOs
        public async Task<IEnumerable<StateDto>> GetAllStatesAsync()
        {
            try
            {
                var states = await _stateData.GetAllAsync();
                return states.Select(state => new StateDto
                {
                    Id = state.Id,
                    Name = state.Name,
                    Description = state.Description,
                    CreatedAt = state.CreatedAt,
                    UpdatedAt = state.UpdatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los estados");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de estados", ex);
            }
        }

        // Método para obtener un estado por ID como DTO
        public async Task<StateDto> GetStateByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un estado con ID inválido: {StateId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del estado debe ser mayor que cero");
            }

            try
            {
                var state = await _stateData.GetByIdAsync(id);
                if (state == null)
                {
                    _logger.LogInformation("No se encontró ningún estado con ID: {StateId}", id);
                    throw new EntityNotFoundException("State", id);
                }

                return new StateDto
                {
                    Id = state.Id,
                    Name = state.Name,
                    Description = state.Description,
                    CreatedAt = state.CreatedAt,
                    UpdatedAt = state.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el estado con ID: {StateId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el estado con ID {id}", ex);
            }
        }

        // Método para crear un estado desde un DTO
        public async Task<StateDto> CreateStateAsync(StateDto stateDto)
        {
            try
            {
                ValidateState(stateDto);

                var state = new State
                {
                    Name = stateDto.Name,
                    Description = stateDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdState = await _stateData.CreateAsync(state);

                return new StateDto
                {
                    Id = createdState.Id,
                    Name = createdState.Name,
                    Description = createdState.Description,
                    CreatedAt = createdState.CreatedAt,
                    UpdatedAt = createdState.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo estado: {StateName}", stateDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el estado", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateState(StateDto stateDto)
        {
            if (stateDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto estado no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(stateDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un estado con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del estado es obligatorio");
            }
        }
    }
}
