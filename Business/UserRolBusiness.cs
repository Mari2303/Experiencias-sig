using Data;

using Entity.Model;
using Entity.DTOs;

using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;
using Entity.ModelExperience;
using ValidationException = Utilities.Exeptions.ValidationException;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles de usuario del sistema.
    /// </summary>
    public class UserRolBusiness
    {
        private readonly UserRolData _userRolData;
        private readonly ILogger<UserRol> _logger;

        public UserRolBusiness(UserRolData userRolData, ILogger<UserRol> logger)
        {
            _userRolData = userRolData;
            _logger = logger;
        }

        // Método para obtener todos los roles de usuario como DTOs
        public async Task<IEnumerable<UserRolDTO>> GetAllUserRolesAsync()
        {
            try
            {
                var userRoles = await _userRolData.GetAllAsync();
             

                return MapToDTOList(userRoles);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles de usuario");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles de usuario", ex);
            }
        }

        // Método para obtener un rol de usuario por ID como DTO
        public async Task<UserRolDTO> GetUserRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol de usuario con ID inválido: {UserRolId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del rol de usuario debe ser mayor que cero");
            }

            try
            {
                var userRol = await _userRolData.GetByIdAsync(id);
                if (userRol == null)
                {
                    _logger.LogInformation("No se encontró ningún rol de usuario con ID: {UserRolId}", id);
                    throw new EntityNotFoundException("UserRol", id);
                }

                
                return MapToDTO(userRol);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol de usuario con ID: {UserRolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol de usuario con ID {id}", ex);
            }
        }

        // Método para crear un rol de usuario desde un DTO
        public async Task<UserRolDTO> CreateUserRolAsync(UserRolDTO UserRolDTO)
        {
            try
            {
                ValidateUserRol(UserRolDTO);


                var UserRol = MapToEntity(UserRolDTO);

                var CreatedUserRol = await _userRolData.CreateAsync(UserRol);

              return MapToDTO(CreatedUserRol);




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol de usuario: {UserRolNombre}", UserRolDTO?.RolId);
                throw new ExternalServiceException("Base de datos", "Error al crear el rol de usuario", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateUserRol(UserRolDTO UserRolDTO)
        {
            if (UserRolDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto rol de usuario no puede ser nulo");
            }

            if (UserRolDTO.RolId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un RolId con Name vacío:{RolId}", UserRolDTO.RolId);

                throw new Utilities.Exeptions.ValidationException("Name", "El Name del rol de usuario es obligatorio");
            }
        }



        public async Task<bool> PatchUserRolAsync(int id, int rolId, int userId)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            return await _userRolData.PatchUserRolAsync(id, rolId, userId)
                ?? throw new EntityNotFoundException("UserRol", id);
        }

        public async Task<bool> PutUserRolAsync(int id, UserRolDTO dto)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");
            if (dto == null)
                throw new ValidationException("UserRol", "Datos inválidos.");

            return await _userRolData.PutUserRolAsync(id, dto.RolId, dto.UserId)
                ?? throw new EntityNotFoundException("UserRol", id);
        }




        // Método para eliminar permanentemente el rol de usuario
        public async Task<bool> DeletePermanentAsync(int id)
        {
            try
            {
                return await _userRolData.DeletePermanentAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar permanentemente el rol de usuario con ID {id}: {ex.Message}");
                return false;
            }
        }









        // Metodo para mapear la entidad a DTO

        private UserRolDTO MapToDTO(UserRol userRol)
        {
            return new UserRolDTO
            {
                Id = userRol.Id,
                RolId = userRol.RolId,
                UserId = userRol.UserId
             
            };
        }

        // Método para mapear el DTO a la entidad

        private UserRol MapToEntity(UserRolDTO userRolDTO)
        {
            return new UserRol
            {
                Id = userRolDTO.Id,
                RolId = userRolDTO.RolId,
                UserId = userRolDTO.UserId
            };
        }

        // Método para mapear una lista de entidades a una lista de DTOs

        private List<UserRolDTO> MapToDTOList(IEnumerable<UserRol> userRoles)
        {
            var userRolesDTO = new List<UserRolDTO>();
            foreach (var userRol in userRoles)
            {
                userRolesDTO.Add(MapToDTO(userRol));
            }
            return userRolesDTO;
        }

    }
}
