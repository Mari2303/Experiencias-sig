using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class UserBusiness
    {
        private readonly UserData _UserData;
        private readonly ILogger _logger;

        public UserBusiness(UserData UserData, ILogger logger)
        {
            _UserData = UserData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<UserDTO>> GetAllRolesAsync()
        {
            try
            {
                var User = await _UserData.GetAllAsync();
                var UserDTO = new List<UserDto>();

                foreach (var User in User)
                {
                    UserDTO.Add(new UserDto
                    {
                        Id = User.Id,
                        Name = User.Name,
                        Active = User.Active // Si existe en la entidad
                    });
                }

                return UserDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<UserDto> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un User con ID inválido: {UserId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del User debe ser mayor que cero");
            }

            try
            {
                var User = await _UserData.GetByIdAsync(id);
                if (User == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {UserId}", id);
                    throw new EntityNotFoundException("User", id);
                }

                return new UserDto
                {
                    Id = User.Id,
                    Name = User.Name,
                    Active = User.Active 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el User con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el User con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<UserDto> CreateRolAsync(UserDto UserDto)
        {
            try
            {
                ValidateRol(UserDto);

                var User = new User
                {
                    Name = UserDto.Name,
                    Active = UserDto.Active // Si existe en la entidad
                };

                var UserCreado = await _UserData.CreateAsync(User);

                return new UserDto
                {
                    Id = UserCreado.Id,
                    Name = UserCreado.Name,
                    Active = UserCreado.Active // Si existe en la entidad
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo User: {UserNombre}", UserDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el User", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateUser(UserDTO UserDTO)
        {
            if (UserDTO == null)
    
                throw new Utilities.Exceptions.ValidationException("El objeto User no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(UserDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un User con Name vacío");

                throw new Utilities.Exceptions.ValidationException("Name", "El Name del User es obligatorio");
            }
        }
    

