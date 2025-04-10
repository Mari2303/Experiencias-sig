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
    public class RolBusiness
    {
        private readonly RolData _rolData;
        private readonly ILogger _logger;

        public RolBusiness(RolData rolData, ILogger logger)
        {
            _rolData = rolData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<RolDTO>> GetAllRolesAsync()
        {
            try
            {

                var roles = await _rolData.GetAllAsync();

                return MapToDTOList(roles);
                




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<RolDTO> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
                throw new ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var rol = await _rolData.GetByIdAsync(id);
                if (rol == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }



                return MapToDTO(rol);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<RolDTO> CreateRolAsync(RolDTO RolDTO)
        {
            try
            {
                ValidateRol(RolDTO);

                var rol = MapToEntity(RolDTO);

                var rolCreate = await _rolData.CreateAsync(rol);

                return MapToDTO(rolCreate);
            

            

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", RolDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRol(RolDTO RolDTO)
        {
            if (RolDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(RolDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }







        // Método para mapear de Rol a RolDTO
        private RolDTO MapToDTO(Rol rol)
        {
            return new RolDTO
            {
                Id = rol.Id,
                Name = rol.Name,
                typeRol = rol.typeRol, // Si existe en la entidad
                Active = rol.Active
            };
        }

        // Método para mapear de RolDTO a Rol
        private Rol MapToEntity(RolDTO rolDTO)
        {
            return new Rol
            {
                Id = rolDTO.Id,
                Name = rolDTO.Name,
                typeRol = rolDTO.typeRol, // Si existe en la entidad
                Active = rolDTO.Active
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<RolDTO> MapToDTOList(IEnumerable<Rol> roles)
        {
            var rolesDTO = new List<RolDTO>();
            foreach (var rol in roles)
            {
                rolesDTO.Add(MapToDTO(rol));
            }
            return rolesDTO;
        }
    }
}
