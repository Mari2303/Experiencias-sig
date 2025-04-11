using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con la población de experiencias en el sistema.
    /// </summary>
    public class ExperiencePopulationBusiness
    {
        private readonly ExperiencePopulationData _experiencePopulationData;
        private readonly ILogger _logger;

        public ExperiencePopulationBusiness(ExperiencePopulationData experiencePopulationData, ILogger logger)
        {
            _experiencePopulationData = experiencePopulationData;
            _logger = logger;
        }

        // Método para obtener todas las poblaciones de experiencia como DTOs
        public async Task<IEnumerable<ExperienciePopulationDTO>> GetAllExperiencePopulationsAsync()
        {
            try
            {
                var ExperienciePopulations = await _experiencePopulationData.GetAllAsync();


                return MapToDTOList(ExperienciePopulations);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las poblaciones de experiencia");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de poblaciones de experiencia", ex);
            }
        }

        // Método para obtener una población de experiencia por ID como DTO
        public async Task<ExperienciePopulationDTO> GetExperiencePopulationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una población de experiencia con ID inválido: {PopulationId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID de la población de experiencia debe ser mayor que cero");
            }

            try
            {
                var population = await _experiencePopulationData.GetByIdAsync(id);
                if (population == null)
                {
                    _logger.LogInformation("No se encontró ninguna población de experiencia con ID: {PopulationId}", id);
                    throw new EntityNotFoundException("ExperiencePopulation", id);
                }


                return MapToDTO(population);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la población de experiencia con ID: {PopulationId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la población de experiencia con ID {id}", ex);
            }
        }

        // Método para crear una población de experiencia desde un DTO
        public async Task<ExperienciePopulationDTO> CreateExperiencePopulationAsync(ExperienciePopulationDTO populationDTO)
        {
            try
            {
                ValidateExperiencePopulation(populationDTO);


                var population = MapToEntity(populationDTO);

                var createdPopulation = await _experiencePopulationData.CreateAsync(population);

                return MapToDTO(createdPopulation);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva población de experiencia: {PopulationName}", populationDTO?.PopulationGradeName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la población de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperiencePopulation(ExperienciePopulationDTO populationDto)
        {
            if (populationDto == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto población de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(populationDto.ExperiencieName))
            {
                _logger.LogWarning("Se intentó crear/actualizar una población de experiencia con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name de la población de experiencia es obligatorio");
            }
        }


        // Método para mapear una entidad a un DTO
        
        private ExperienciePopulationDTO MapToDTO(ExperiencePopulation population)
        {
            return new ExperienciePopulationDTO
            {
                Id = population.Id,
               ExperiencieId = population.ExperiencieId,
                PopulationGradeId = population.PopulationGradeId,
                
            };
        }

        // Metodo para mapear de DTO a entidad

        private ExperiencePopulation MapToEntity(ExperienciePopulationDTO populationDto)
        {
            return new ExperiencePopulation
            {
                Id = populationDto.Id,
                ExperiencieId = populationDto.ExperiencieId,
                PopulationGradeId = populationDto.PopulationGradeId,
             
            };
        }

        // Método para mapear una lista de entidades a una lista de DTOs
        private IEnumerable<ExperienciePopulationDTO> MapToDTOList(IEnumerable<ExperiencePopulation> populations)
        {      
            var populationDtos = new List<ExperienciePopulationDTO>();

            foreach (var population in populations)
            {
                populationDtos.Add(MapToDTO(population));
            }
            return populationDtos;




        }
    }
}
