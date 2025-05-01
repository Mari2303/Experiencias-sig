using Data;
using Entity.Model;
using Entity.DTOs;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;
using ValidationException = Utilities.Exeptions.ValidationException;
namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class UserBusiness
    {
        private readonly UserData _UserData;
        private readonly ILogger<User> _logger;

        public UserBusiness(UserData UserData, ILogger<User> logger)
        {
            _UserData = UserData;
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
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
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



        // Método para crear un rol desde un DTO
        public async Task<UserDTO> CreateUserAsync(UserDTO UserDTO)
        {
            try
            {
                ValidateUser(UserDTO);

                var User = MapToEntity(UserDTO);
                
                var createdUser = await _UserData.CreateAsync(User);

                return MapToDTO(createdUser);

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo User: {UserNombre}", UserDTO?.Email ?? "null");
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




        public async Task<User?> LoginAsync(string email, string password)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Email o contraseña vacíos al intentar login.");
                throw new ValidationException("Email o Password", "Ambos campos son obligatorios.");
            }

            try
            {
                var users = await _UserData.GetAllAsync(); // Asumiendo que este método ya existe

                var user = users.FirstOrDefault(u =>
                    u.Email == email &&
                    u.Password == password &&
                    u.Active == true
                );

                if (user == null)
                {
                    _logger.LogInformation("Intento fallido de login para el email: {Email}", email);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar login con el email: {Email}", email);
                throw new ExternalServiceException("Login", "Ocurrió un error al intentar iniciar sesión.", ex);
            }
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
