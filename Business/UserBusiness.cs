using Data;
using Entity.Model;
using Entity.DTOs;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;
using ValidationException = Utilities.Exeptions.ValidationException;
using BCrypt.Net;
using System.Text.RegularExpressions;
using Entity.ModelExperience;
namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class UserBusiness
    {
        private readonly UserData _UserData;
        private readonly RolData _RolData;
        private readonly PermissionData _PermissionData;
        private readonly RolPermissionData _RolPermissionData;
        private readonly UserRolData _UserRolData;
        private readonly ILogger<User> _logger;

        public UserBusiness(UserData UserData, RolData RolData, RolPermissionData RolPermissionData, UserRolData UserRolData, PermissionData PermissionData, ILogger<User> logger)
        {
            _UserData = UserData;
            _RolData = RolData;
            _RolPermissionData = RolPermissionData;
            _PermissionData = PermissionData;
            _UserRolData = UserRolData;
            _logger = logger;
        }






        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<UserDTO>> GetAllUserAsync()
        {
            try
            {
                var User = await _UserData.GetAllAsync();

                return MapToDTOList(User);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los user");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de user", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un User con ID inválido: {UserId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del User debe ser mayor que cero");
            }

            try
            {
                var User = await _UserData.GetByIdAsync(id);
                if (User == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {UserId}", id);
                    throw new EntityNotFoundException("User", id);
                }


                return MapToDTO(User);



            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el User con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el User con ID {id}", ex);
            }
        }



        // Método para crear un user desde un DTO
        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            try
            {
                ValidateUser(userDTO);

                var user = MapToEntity(userDTO);

                // 🔐 Encriptar la contraseña antes de guardar
                user.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

                var createdUser = await _UserData.CreateAsync(user);

                // Asignar rol predeterminado
                var defaultRole = await _RolData.GetByNameAsync("Profesor");
                if (defaultRole != null)
                {
                    var userRol = new UserRol
                    {
                        UserId = createdUser.Id,
                        RolId = defaultRole.Id
                    };
                    await _UserRolData.CreateAsync(userRol);
                }

                // Asignar permiso predeterminado (por ejemplo: "Ver")
              
                // Por esta línea:
                var defaultPermission = await _PermissionData.GetByTypeAsync("Ver");

                if (defaultPermission != null)
                {
                    var rolePermission = new RolPermission
                    {
                        RolId = defaultRole.Id,
                        PermissionId = defaultPermission.Id
                    };

                    // Opcional: Verifica si ya existe esa relación antes de insertar
                    await _RolPermissionData.CreateIfNotExistsAsync(rolePermission);
                }

                return MapToDTO(createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo User: {UserEmail}", userDTO?.Email ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el User", ex);
            }
        }


        // Método para validar el DTO
        private void ValidateUser(UserDTO UserDTO)
        {
            if (UserDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto  puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(UserDTO.Email))
            {
                _logger.LogWarning("Se intentó crear/actualizar con email vacío");
                throw new Utilities.Exeptions.ValidationException("email", "El Name es obligatorio");
            }
        }



        public async Task<bool> UpdatePartialAsync(int id, string email, string password, bool active, int personId)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException("name", "El nombre es obligatorio.");

            var result = await _UserData.UpdatePartialAsync(id,  email, password, active, personId);
            if (!result)
                throw new EntityNotFoundException("User", id);

            return true;
        }

        public async Task<bool> UpdateFullAsync(int id, UserDTO dto)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (dto == null)
                throw new ValidationException("User", "Datos inválidos.");

            var result = await _UserData.UpdateFullAsync(id, dto.Email, dto.Password, dto.Active, dto.PersonId);
            if (!result)
                throw new EntityNotFoundException("User", id);

            return true;
        }

        public async Task<bool> DeleteLogicalAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            var deleted = await _UserData.DeleteLogicalAsync(id);
            if (!deleted)
                throw new EntityNotFoundException("User", id);

            return true;
        }




            public async Task<bool> DeleteAsync(int id)
            {
                try
                {
                    return await _UserData.DeleteAsync(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error en la capa Business al eliminar permanentemente el usuario con ID {id}");
                    throw;
                }
            }










        public async Task<UserDTO> ValidateCredentialsAsync(string email, string password)
        {
            var user = await _UserData.GetByEmailAsync(email);

            // Validar si el usuario existe y está activo
            if (user == null || !user.Active)
                return null;

            // Comparar contraseñas usando BCrypt
            bool passwordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!passwordValid)
            {
                _logger.LogWarning("Contraseña incorrecta para el usuario con email {Email}.", email);
                return null;
            }

            return MapToDTO(user);
        }








        // Metodo para mapear de una entidad  UN DTO

        private UserDTO MapToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
          
                Email = user.Email,
                Password = user.Password,
                PersonId = user.PersonId,
              
            };
        }

        // Metodo para mapear de un DTO a una entidad

        private User MapToEntity(UserDTO userDTO)
        {
            return new User
            {
                Id = userDTO.Id,
              
                Email = userDTO.Email,
                Password = userDTO.Password,
                PersonId = userDTO.PersonId
            };
        }


        // Método para mapear una lista de entidades a una lista de DTOs

        
        private IEnumerable<UserDTO> MapToDTOList(IEnumerable<User> user)
        {
            var userDTO = new List<UserDTO>();
            foreach (var User in user)
            {
                userDTO.Add(MapToDTO(User));
            }
            return userDTO;
        }
    }
}
