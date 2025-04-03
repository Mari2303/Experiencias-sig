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
                var rolPermissionsDTO = new List<RolPermissionDTO>();

                foreach (var rolper in rolPermissionsDTO)
                {
                    rolPermissionsDTO.Add(new RolPermissionDTO

                    {
                        Id = rolper.Id,
                        RolId = rolper.RolId,
                        PermissionId = rolper.PermissionId

                    });
                }
                return rolPermissionsDTO;

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

                return new RolPermissionDTO
                {
                   Id = rolPermision.Id,
                  RolId = rolPermision.RolId,
                  PermissionId = rolPermision.PermissionId
                };
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

                var rolPermision = new RolPermission
                {
                    RolId = RolPermissionDTO.RolId,
                    PermissionId = RolPermissionDTO.PermisionId
                };

                var createdRolPermision = await _rolPermisionData.CreateAsync(rolPermision);

                return new RolPermissionDTO
                {
                    Id = createdRolPermision.Id,
                    RolId = createdRolPermision.RolId,
                    PermissionId = createdRolPermision.PermissionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo : {Nombre}", RolPermissionDTO?.name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear ", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRolPermissionDTO(RolPermissionDTO rolPermissionDTO)
        {
            if (RolPermissionDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(RolPermissionDTO.name))
            {
                _logger.LogWarning("Se intentó crear/actualizar  con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name  obligatorio");
            }
        }
    }
}
           
        