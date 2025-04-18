
using Data;
using Entity.Model;
using Entity.DTOs;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;
using Microsoft.Extensions.Logging;


/// <summary>
/// Clase de negocio encargada de la lógica relacionada con los estados en el sistema.
/// </summary>
public class StateBusiness
{
    private readonly StateData _stateData;
    private readonly ILogger<State> _logger;

    public StateBusiness(StateData stateData, ILogger<State> logger)
    {
        _stateData = stateData;
        _logger = logger;
    }

    // Método para obtener todos los estados como DTOs
    public async Task<IEnumerable<StateDTO>> GetAllStatesAsync()
    {
        try
        {
            var states = await _stateData.GetAllAsync();

            return MapToDTOList(states);

        }


        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los estados");
            throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de estados", ex);
        }
    }

    // Método para obtener un estado por ID como DTO
    public async Task<StateDTO> GetStateByIdAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("Se intentó obtener un estado con ID inválido: {StateId}", id);
            throw new Utilities.Exeptions.ValidationException("id", "El ID del estado debe ser mayor que cero");
        }

        try
        {
            var state = await _stateData.GetByIdAsync(id);
            if (state == null)
            {
                _logger.LogInformation("No se encontró ningún estado con ID: {StateId}", id);
                throw new EntityNotFoundException("State", id);
            }


            return MapToDTO(state);



        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el estado con ID: {StateId}", id);
            throw new ExternalServiceException("Base de datos", $"Error al recuperar el estado con ID {id}", ex);
        }
    }

    // Método para crear un estado desde un DTO
    public async Task<StateDTO> CreateStateAsync(StateDTO StateDTO)
    {
        try
        {


            ValidateState(StateDTO);

            var state = MapToDTOEntity(StateDTO);

            var createdState = await _stateData.CreateAsync(state);

            return MapToDTO(createdState);



        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear un nuevo estado: {StateName}", StateDTO?.Name ?? "null");
            throw new ExternalServiceException("Base de datos", "Error al crear el estado", ex);
        }
    }

    // Método para validar el DTO
    private void ValidateState(StateDTO StateDTO)
    {
        if (StateDTO == null)
        {
            throw new Utilities.Exeptions.ValidationException("El objeto estado no puede ser nulo");
        }

        if (string.IsNullOrWhiteSpace(StateDTO.Name))
        {
            _logger.LogWarning("Se intentó crear/actualizar un estado con Name vacío");
            throw new Utilities.Exeptions.ValidationException("Name", "El Name del estado es obligatorio");
        }
    }


    // Metodo para mapear de la entidad a DTO 

    private StateDTO MapToDTO(State state)
    {
        return new StateDTO
        {
            Id = state.Id,
            Name = state.Name
        };
    }


    // Método para mapear de DTO a entidad
    private State MapToDTOEntity(StateDTO stateDTO)
    {
        return new State
        {
            Id = stateDTO.Id,
            Name = stateDTO.Name
        };
    }

    // Método para mapear una lista de entidades a DTOs

    private IEnumerable<StateDTO> MapToDTOList(IEnumerable<State> states)
    {
        var statesList = new List<StateDTO>();
        foreach (var state in states)
        {
        }

        return statesList;

    }

}