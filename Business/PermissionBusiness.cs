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
        private readonly ILogger<Permission> _logger;

        public PermissionBusiness(PermissionData permissionData, ILogger<Permission> logger)
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


              
                return MapToDTOList(permisions);


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

               

                return MapToDTO(permision);




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID: {PermisionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso con ID {id}", ex);
            }
        }

        // Método para crear un permiso desde un DTO
        public async Task<PermissionDTO> CreatePermisionAsync(PermissionDTO permisionDTO)
        {
            try
            {
                ValidatePermision(permisionDTO);

                var permision = MapToEntity(permisionDTO);

                var createdPermision = await _permisionData.CreateAsync(permision);

                return MapToDTO(createdPermision);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo permiso: {PermisionName}", permisionDTO?.PermissionType ?? "null");
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

        //Metodo para mapear de entidades a DTO 

        private PermissionDTO MapToDTO(Permission permission)
        {
            return new PermissionDTO
            {
                Id = permission.Id,
                PermissionType = permission.PermissionType
            };
        }

        // Metodo para mapear de DTO a entidad
        private Permission MapToEntity(PermissionDTO permissionDTO)
        {
            return new Permission
            {
                Id = permissionDTO.Id,
                PermissionType = permissionDTO.PermissionType
            };
        }


        // Método para mapear una lista de entidades a DTOs
        private IEnumerable<PermissionDTO> MapToDTOList(IEnumerable<Permission> permissions)
        {
          var permissionDTOs = new List<PermissionDTO>();
            foreach (var permission in permissions)
            {
                permissionDTOs.Add(MapToDTO(permission));
            }
            return permissionDTOs;
        }

    }
}
