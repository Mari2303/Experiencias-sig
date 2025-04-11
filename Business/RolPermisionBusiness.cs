using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los permisos de roles en el sistema.
    /// </summary>
    public class RolPermisionBusiness
    {
        private readonly RolPermissionData  _rolPermisionData;
        private readonly ILogger _logger;

        public RolPermisionBusiness(RolPermissionData rolPermisionData, ILogger logger)
        {
            _rolPermisionData = rolPermisionData;
            _logger = logger;
        }

        // Método para obtener todos los permisos de roles como DTOs
        public async Task<IEnumerable<RolPermissionDTO>> GetAllRolPermissionsAsync()
        {
            try
            {
                var rolPermissions = await _rolPermisionData.GetAllAsync();
           

                return MapToDTOList(rolPermissions);


           

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos de roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos de roles", ex);
            }
        }

        // Método para obtener un permiso de rol por ID como DTO
        public async Task<RolPermissionDTO> GetRolPermisionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un permiso de rol con ID inválido: {RolPermisionId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del permiso de rol debe ser mayor que cero");
            }

            try
            {
                var rolPermision = await _rolPermisionData.GetByIdAsync(id);
                if (rolPermision == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso con ID: {RolPermisionId}", id);
                    throw new EntityNotFoundException("RolPermision", id);
                }



                return MapToDTO(rolPermision);




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso  con ID: {RolPermisionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso de rol con ID {id}", ex);
            }
        }

        // Método para crear un permiso de rol desde un DTO
        public async Task<RolPermissionDTO> CreateRolPermisionAsync(RolPermissionDTO RolPermissionDTO)
        {
            try
            {
                ValidateRolPermissionDTO(RolPermissionDTO);

                var rolPermission = MapToEntity(RolPermissionDTO);
                   
               var createdRolPermission = await _rolPermisionData.CreateAsync(rolPermission);

              return MapToDTO(createdRolPermission);
 


    }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo : {Nombre}", RolPermissionDTO?.RolId );
                throw new ExternalServiceException("Base de datos", "Error al crear ", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRolPermissionDTO(RolPermissionDTO RolPermissionDTO)
        {
            if (RolPermissionDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto puede ser nulo");
            }

            if (RolPermissionDTO.RolId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar  con RolId inválido: {RolId}", RolPermissionDTO.RolId);
                throw new Utilities.Exeptions.ValidationException("Name", "El Name  obligatorio");
            }
        }


// Metodo para mapear de entidad  DTO
private RolPermissionDTO MapToDTO(RolPermission rolPermission)
{
    return new RolPermissionDTO
    {
        Id = rolPermission.Id,
        RolId = rolPermission.RolId,
        PermissionId = rolPermission.PermissionId,
       
    };
}


// Metodo para mapear de DTO a entidad 

private RolPermission MapToEntity(RolPermissionDTO rolPermissionDTO)

{
    return new RolPermission
    {
        Id = rolPermissionDTO.Id,
        RolId = rolPermissionDTO.RolId,
        PermissionId = rolPermissionDTO.PermissionId,

    };
}


// Método para mapear una lista de entidades a DTOs


private IEnumerable<RolPermissionDTO> MapToDTOList(IEnumerable<RolPermission> rolPermissions)

{
    var rolPermissionsDTO = new List<RolPermissionDTO>();
    foreach (var rolPermission in rolPermissions)
    {
        rolPermissionsDTO.Add(MapToDTO(rolPermission));
    }

    return rolPermissionsDTO; 


}

    }
}
           
        