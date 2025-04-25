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
        public async Task<UserDTO> GetRolByIdAsync(int id)
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
        public async Task<UserDTO> CreateUserlAsync(UserDTO UserDTO)
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
                _logger.LogError(ex, "Error al crear nuevo User: {UserNombre}", UserDTO?.Name ?? "null");
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

            if (string.IsNullOrWhiteSpace(UserDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name es obligatorio");
            }
        }



        public async Task<bool> UpdatePartialAsync(int id, string name, string email, string password, bool active, int personId)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("name", "El nombre es obligatorio.");

            var result = await _UserData.UpdatePartialAsync(id, name, email, password, active, personId);
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

            var result = await _UserData.UpdateFullAsync(id, dto.Name, dto.Email, dto.Password, dto.Active, dto.PersonId);
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























        // Metodo para mapear de una entidad  UN DTO

        private UserDTO MapToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
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
                Name = userDTO.Name,
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
