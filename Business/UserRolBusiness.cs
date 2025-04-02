using Data;

using Entity.Model;
using Entity.DTOs;

using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;
using Entity.ModelExperience;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles de usuario del sistema.
    /// </summary>
    public class UserRolBusiness
    {
        private readonly UserRolData _userRolData;
        private readonly ILogger _logger;

        public UserRolBusiness(UserRolData userRolData, ILogger logger)
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
                var userRolesDTO = new List<UserRolDTO>();

                foreach (var userRol in userRoles)
                {
                    userRolesDTO.Add(new UserRolDTO
                    {
                        Id = userRol.Id,
                        RolId = userRol.RolId,
                        UserId = userRol.UserId
                    });
                }

                return userRolesDTO;
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

                return new UserRolDTO
                {
                    Id = userRol.Id,
                    RolId = userRol.RolId,
                    UserId = userRol.UserId
                };
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

                var userRol = new UserRol
                {
                 
                    RolId = UserRolDTO.RolId,
                    UserId = UserRolDTO.UserId
                };

                var userRolCreado = await _userRolData.CreateAsync(userRol);

                return new UserRolDTO
                {
                    Id = userRolCreado.Id,
                    RolId = userRolCreado.RolId,
                    UserId = userRolCreado.UserId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol de usuario: {UserRolNombre}", UserRolDTO?.Name ?? "null");
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

            if (string.IsNullOrWhiteSpace(UserRolDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol de usuario con Name vacío");

                throw new Utilities.Exeptions.ValidationException("Name", "El Name del rol de usuario es obligatorio");
            }
        }
    }
}
