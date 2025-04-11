using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las instituciones en el sistema.
    /// </summary>
    public class InstitucionBusiness
    {
        private readonly InstitucionData _institucionData;
        private readonly ILogger _logger;

        public InstitucionBusiness(InstitucionData institucionData, ILogger logger)
        {
            _institucionData = institucionData;
            _logger = logger;
        }

        // Método para obtener todas las instituciones como DTOs
        public async Task<IEnumerable<InstitucionDTO>> GetAllInstitucionesAsync()
        {
            try
            {
                var instituciones = await _institucionData.GetAllAsync();

                return MapToDTOList(instituciones);




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las instituciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de instituciones", ex);
            }
        }

        // Método para obtener una institución por ID como DTO
        public async Task<InstitucionDTO> GetInstitucionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una institución con ID inválido: {InstitucionId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID de la institución debe ser mayor que cero");
            }

            try
            {
                var institucion = await _institucionData.GetByIdAsync(id);
                if (institucion == null)
                {
                    _logger.LogInformation("No se encontró ninguna institución con ID: {InstitucionId}", id);
                    throw new EntityNotFoundException("Institucion", id);
                }



                return MapToDTO(institucion);




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la institución con ID: {InstitucionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la institución con ID {id}", ex);
            }
        }

        // Método para crear una institución desde un DTO
        public async Task<InstitucionDTO> CreateInstitucionAsync(InstitucionDTO InstitucionDTO)
        {
            try
            {
                ValidateInstitucion(InstitucionDTO);


                var institucion = MapToEntity(InstitucionDTO);

                var institucionCreate = await _institucionData.CreateAsync(institucion);

                return MapToDTO(institucionCreate);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva institución: {InstitucionName}", InstitucionDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la institución", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateInstitucion(InstitucionDTO InstitucionDTO)
        {
            if (InstitucionDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto institución no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(InstitucionDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una institución con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name de la institución es obligatorio");
            }
        }



        // Metodo para mapear una entidad a un DTO

        private InstitucionDTO MapToDTO(Institucion inst)
        {
            return new InstitucionDTO
            {
                Id = inst.Id,
                Name = inst.Name,
                Address = inst.Address,
                Phone = inst.Phone,
                EmailInstitution = inst.EmailInstitution,
                Department = inst.Department,
                Commune = inst.Commune
            };
        }


        //Metodo para mapear una DTO a una entidad


        private Institucion MapToEntity(InstitucionDTO instDTO)
        {
            return new Institucion
            {
                Id = instDTO.Id,
                Name = instDTO.Name,
                Address = instDTO.Address,
                Phone = instDTO.Phone,
                EmailInstitution = instDTO.EmailInstitution,
                Department = instDTO.Department,
                Commune = instDTO.Commune
            };
        }




        // Método para mapear una lista de entidades a una lista de DTOs

        private IEnumerable<InstitucionDTO> MapToDTOList(IEnumerable<Institucion> instituciones)
        {

            var institucionDTOs = new List<InstitucionDTO>();
            foreach (var inst in instituciones)
            {
                institucionDTOs.Add(MapToDTO(inst));
            }

            return institucionDTOs;
        }
    }
}
