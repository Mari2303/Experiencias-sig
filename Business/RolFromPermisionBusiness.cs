using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;
using ValidationException = Utilities.Exeptions.ValidationException;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los permisos de roles en el sistema.
    /// </summary>
    public class RolFromPermisionBusiness
    {
        private readonly RolFromPermissionData  _rolPermisionData;
        private readonly ILogger<RolFromPermission> _logger;

        public RolFromPermisionBusiness(RolFromPermissionData rolPermisionData, ILogger<RolFromPermission> logger)
        {
            _rolPermisionData = rolPermisionData;
            _logger = logger;
        }

        // Método para obtener todos los permisos de roles como DTOs
        public async Task<IEnumerable<RolFromPermissionDTO>> GetAllRolPermissionsAsync()
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
        public async Task<RolFromPermissionDTO> GetRolPermisionByIdAsync(int id)
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
        public async Task<RolFromPermissionDTO> CreateRolPermisionAsync(RolFromPermissionDTO RolPermissionDTO)
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
        private void ValidateRolPermissionDTO(RolFromPermissionDTO RolPermissionDTO)
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




        public async Task<bool> PatchRolPermissionAsync(int id, int rolId, int permissionId, int FromId)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            var result = await _rolPermisionData.PatchRolPermissionAsync(id, rolId, permissionId, FromId);
            if (!result)
                throw new EntityNotFoundException("RolPermission", id);

            return true;
        }

        public async Task<bool> PutRolPermissionAsync(int id, RolFromPermissionDTO dto)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (dto == null)
                throw new ValidationException("RolPermission", "Datos inválidos.");

            var result = await _rolPermisionData.PutRolPermissionAsync(id, dto.RolId, dto.PermissionId, dto.FromId);
            if (!result)
                throw new EntityNotFoundException("RolPermission", id);

            return true;
        }






        public async Task<bool> DeletePermanentAsync(int id)
        {
            return await _rolPermisionData.DeletePermanentAsync(id);
        }














        // Metodo para mapear de entidad  DTO
        private RolFromPermissionDTO MapToDTO(RolFromPermission rolPermission)
{
    return new RolFromPermissionDTO
    {
        Id = rolPermission.Id,
        RolId = rolPermission.RolId,
        FromId = rolPermission.FormId,
        PermissionId = rolPermission.PermissionId
      

    };
}


// Metodo para mapear de DTO a entidad 

private RolFromPermission MapToEntity(RolFromPermissionDTO rolPermissionDTO)

{
    return new RolFromPermission
    {
        Id = rolPermissionDTO.Id,
        RolId = rolPermissionDTO.RolId,
        FormId = rolPermissionDTO.FromId,
        PermissionId = rolPermissionDTO.PermissionId

    };
}


// Método para mapear una lista de entidades a DTOs


private IEnumerable<RolFromPermissionDTO> MapToDTOList(IEnumerable<RolFromPermission> rolPermissions)

{
    var rolPermissionsDTO = new List<RolFromPermissionDTO>();
    foreach (var rolPermission in rolPermissions)
    {
        rolPermissionsDTO.Add(MapToDTO(rolPermission));
    }

    return rolPermissionsDTO; 


}

    }
}
           
        