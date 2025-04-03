using Data;

using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exeptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los permisos en el sistema.
    /// </summary>
    public class PermissionBusiness
    {
        private readonly PermissionData _permisionData;
        private readonly ILogger _logger;

        public PermissionBusiness(PermissionData permissionData, ILogger logger)
        {
            _permisionData = permissionData;
            _logger = logger;
        }

        // Método para obtener todos los permisos como DTOs
        public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync()
        {
            try
            {
                var permisions = await _permisionData.GetAllAsync();
                var permisionDTOs = new List<PermissionDTO>();

                foreach (var Permission in permisions)
                {

                    PermissionDTO.Add(new PermissionDTO

                    {
                        Id = Permission.Id,
                        PermissionType = Permission.PermissionType

                    });
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos", ex);
            }
        }

        // Método para obtener un permiso por ID como DTO
        public async Task<PermissionDTO> GetPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un permiso con ID inválido: {PermisionId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del permiso debe ser mayor que cero");
            }

            try
            {
                var permision = await _permisionData.GetByIdAsync(id);
                if (permision == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso con ID: {PermisionId}", id);
                    throw new EntityNotFoundException("Permision", id);
                }

                return new PermissionDTO
                {
                    Id = permision.Id,
                    PermissionType = permision.PermissionType
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID: {PermisionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso con ID {id}", ex);
            }
        }

        // Método para crear un permiso desde un DTO
        public async Task<PermissionDTO> CreatePermisionAsync(PermissionDTO permisionDto)
        {
            try
            {
                ValidatePermision(permisionDto);

                var permision = new Permission
                {
                    PermissionType = permisionDto.PermissionType,
                };

                var createdPermision = await _permisionData.CreateAsync(permision);

                return new PermissionDTO
                {
                    Id = createdPermision.Id,
                    PermissionType = permision.PermissionType
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo permiso: {PermisionName}", permisionDto?.PermissionType ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePermision(PermissionDTO permisionDto)
        {
            if (permisionDto == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto permiso no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(permisionDto.PermissionType))
            {
                _logger.LogWarning("Se intentó crear/actualizar un permiso con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del permiso es obligatorio");
            }
        }
    }
}
