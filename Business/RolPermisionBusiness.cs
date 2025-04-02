using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los permisos de roles en el sistema.
    /// </summary>
    public class RolPermisionBusiness
    {
        private readonly RolPermisionData _rolPermisionData;
        private readonly ILogger _logger;

        public RolPermisionBusiness(RolPermisionData rolPermisionData, ILogger logger)
        {
            _rolPermisionData = rolPermisionData;
            _logger = logger;
        }

        // Método para obtener todos los permisos de roles como DTOs
        public async Task<IEnumerable<RolPermisionDto>> GetAllRolPermisionsAsync()
        {
            try
            {
                var rolPermisions = await _rolPermisionData.GetAllAsync();
                return rolPermisions.Select(rolPermision => new RolPermisionDto
                {
                    Id = rolPermision.Id,
                    RolId = rolPermision.RolId,
                    PermisionId = rolPermision.PermisionId
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos de roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos de roles", ex);
            }
        }

        // Método para obtener un permiso de rol por ID como DTO
        public async Task<RolPermisionDto> GetRolPermisionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un permiso de rol con ID inválido: {RolPermisionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso de rol debe ser mayor que cero");
            }

            try
            {
                var rolPermision = await _rolPermisionData.GetByIdAsync(id);
                if (rolPermision == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso de rol con ID: {RolPermisionId}", id);
                    throw new EntityNotFoundException("RolPermision", id);
                }

                return new RolPermisionDto
                {
                    Id = rolPermision.Id,
                    RolId = rolPermision.RolId,
                    PermisionId = rolPermision.PermisionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso de rol con ID: {RolPermisionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso de rol con ID {id}", ex);
            }
        }

        // Método para crear un permiso de rol desde un DTO
        public async Task<RolPermisionDto> CreateRolPermisionAsync(RolPermisionDto rolPermisionDto)
        {
            try
            {
                ValidateRolPermision(rolPermisionDto);

                var rolPermision = new RolPermision
                {
                    RolId = rolPermisionDto.RolId,
                    PermisionId = rolPermisionDto.PermisionId
                };

                var createdRolPermision = await _rolPermisionData.CreateAsync(rolPermision);

                return new RolPermisionDto
                {
                    Id = createdRolPermision.Id,
                    RolId = createdRolPermision.RolId,
                    PermisionId = createdRolPermision.PermisionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo permiso de rol: {RolId}-{PermisionId}", rolPermisionDto?.RolId, rolPermisionDto?.PermisionId);
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso de rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRolPermision(RolPermisionDto rolPermisionDto)
        {
            if (rolPermisionDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto permiso de rol no puede ser nulo");
            }

            if (rolPermisionDto.RolId <= 0 || rolPermisionDto.PermisionId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un permiso de rol con valores inválidos");
                throw new Utilities.Exceptions.ValidationException("RolId/PermisionId", "Los IDs del rol y permiso deben ser mayores que cero");
            }
        }
    }
}
