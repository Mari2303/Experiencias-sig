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
    /// Clase de negocio encargada de la lógica relacionada con los criterios en el sistema.
    /// </summary>
    public class CriteriaBusiness
    {
        private readonly CriteriaData _criteriaData;
        private readonly ILogger< Criteria> _logger;

        public CriteriaBusiness(CriteriaData criteriaData, ILogger<Criteria> logger)
        {
            _criteriaData = criteriaData;
            _logger = logger;
        }

        // Método para obtener todos los criterios como DTOs
        public async Task<IEnumerable<CriteriaDTO>> GetAllCriteriaAsync()
        {
            try
            {
                var criteria = await _criteriaData.GetAllAsync();


                return MapToDTOList(criteria);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los criterios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de criterios", ex);
            }
        }

        // Método para obtener un criterio por ID como DTO
        public async Task<CriteriaDTO> GetCriteriaByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un criterio con ID inválido: {CriteriaId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del criterio debe ser mayor que cero");
            }

            try
            {
                var criterion = await _criteriaData.GetByIdAsync(id);
                if (criterion == null)
                {
                    _logger.LogInformation("No se encontró ningún criterio con ID: {CriteriaId}", id);
                    throw new EntityNotFoundException("Criteria", id);
                }


                return MapToDTO(criterion);


            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el criterio con ID: {CriteriaId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el criterio con ID {id}", ex);
            }
        }

        // Método para crear un criterio desde un DTO
        public async Task<CriteriaDTO> CreateCriteriaAsync(CriteriaDTO CriteriaDTO)
        {
            try
            {



                ValidateCriteria(CriteriaDTO);

                var criteria = MapToEntity(CriteriaDTO);
                


                var createdCriteria = await _criteriaData.CreateAsync(criteria);

                return MapToDTO(createdCriteria);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo criterio: {CriteriaName}", CriteriaDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el criterio", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateCriteria(CriteriaDTO CriteriaDTO)
        {
            if (CriteriaDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto criterio no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(CriteriaDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un criterio con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del criterio es obligatorio");

            }
        }



        // Metodo patch 

        public async Task<bool> PatchCriteriaNameAsync(int id, string name)
        {
            if (id <= 0)
                throw new Utilities.Exeptions.ValidationException("id", "El ID debe ser mayor que cero.");

            if (string.IsNullOrWhiteSpace(name))
                throw new Utilities.Exeptions.ValidationException("name", "El nombre no puede estar vacío.");

            var criteria = await _criteriaData.GetByIdAsync(id);
            if (criteria == null)
                throw new EntityNotFoundException("Criteria", id);

            criteria.Name = name;

            return await _criteriaData.UpdateAsync(criteria);
        }






        // Metodo put
        public async Task<bool> PutCriteriaAsync(int id, CriteriaDTO dto)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("Criteria", "El nombre del criterio es obligatorio.");

            var result = await _criteriaData.UpdateAsync(new Criteria
            {
                Id = id,
                Name = dto.Name
            });

            if (!result)
                throw new EntityNotFoundException("Criteria", id);

            return true;
        }







        // Método para mapear un DTO a una entidad
        private CriteriaDTO MapToDTO(Criteria criteria)
        {
            return new CriteriaDTO
            {
                Id = criteria.Id,
                Name = criteria.Name,

            };
        }

        // Método para mapear una entidad a un DTO

        private Criteria MapToEntity(CriteriaDTO criteriaDTO)
        {
            return new Criteria
            {
                Id = criteriaDTO.Id,
                Name = criteriaDTO.Name,

            };
        }

        // Método para mapear una lista de entidades a una lista de DTOs
        private IEnumerable<CriteriaDTO> MapToDTOList(IEnumerable<Criteria> criteria)
        {

            var criteriaDTO = new List<CriteriaDTO>();
            foreach (var Criteria in criteria)
            {
                criteriaDTO.Add(MapToDTO(Criteria));
            }
            return criteriaDTO;

        }
    }
}


