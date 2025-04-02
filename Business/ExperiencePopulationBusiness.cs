using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

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
        public async Task<IEnumerable<ExperiencePopulationDto>> GetAllExperiencePopulationsAsync()
        {
            try
            {
                var populations = await _experiencePopulationData.GetAllAsync();
                return populations.Select(pop => new ExperiencePopulationDto
                {
                    Id = pop.Id,
                    Name = pop.Name,
                    Description = pop.Description,
                    CreatedAt = pop.CreatedAt,
                    UpdatedAt = pop.UpdatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las poblaciones de experiencia");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de poblaciones de experiencia", ex);
            }
        }

        // Método para obtener una población de experiencia por ID como DTO
        public async Task<ExperiencePopulationDto> GetExperiencePopulationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una población de experiencia con ID inválido: {PopulationId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la población de experiencia debe ser mayor que cero");
            }

            try
            {
                var population = await _experiencePopulationData.GetByIdAsync(id);
                if (population == null)
                {
                    _logger.LogInformation("No se encontró ninguna población de experiencia con ID: {PopulationId}", id);
                    throw new EntityNotFoundException("ExperiencePopulation", id);
                }

                return new ExperiencePopulationDto
                {
                    Id = population.Id,
                    Name = population.Name,
                    Description = population.Description,
                    CreatedAt = population.CreatedAt,
                    UpdatedAt = population.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la población de experiencia con ID: {PopulationId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la población de experiencia con ID {id}", ex);
            }
        }

        // Método para crear una población de experiencia desde un DTO
        public async Task<ExperiencePopulationDto> CreateExperiencePopulationAsync(ExperiencePopulationDto populationDto)
        {
            try
            {
                ValidateExperiencePopulation(populationDto);

                var population = new ExperiencePopulation
                {
                    Name = populationDto.Name,
                    Description = populationDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdPopulation = await _experiencePopulationData.CreateAsync(population);

                return new ExperiencePopulationDto
                {
                    Id = createdPopulation.Id,
                    Name = createdPopulation.Name,
                    Description = createdPopulation.Description,
                    CreatedAt = createdPopulation.CreatedAt,
                    UpdatedAt = createdPopulation.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva población de experiencia: {PopulationName}", populationDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la población de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperiencePopulation(ExperiencePopulationDto populationDto)
        {
            if (populationDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto población de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(populationDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una población de experiencia con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la población de experiencia es obligatorio");
            }
        }
    }
}
