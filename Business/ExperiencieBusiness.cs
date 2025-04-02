using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las experiencias en el sistema.
    /// </summary>
    public class ExperiencieBusiness
    {
        private readonly ExperiencieData _experiencieData;
        private readonly ILogger _logger;

        public ExperiencieBusiness(ExperiencieData experiencieData, ILogger logger)
        {
            _experiencieData = experiencieData;
            _logger = logger;
        }

        // Método para obtener todas las experiencias como DTOs
        public async Task<IEnumerable<ExperiencieDTO>> GetAllExperiencesAsync()
        {
            try
            {
                var experiencie = await _experiencieData.GetAllAsync();
                var ExperiencieDTOs = new List<ExperiencieDTO>();

                foreach (var experience in experiencie)
                {
                    ExperiencieDTOs.Add(new ExperiencieDTO
                    {
                        Id = experience.Id,
                        NameExperience = experience.NameExperience,
                        Summary = experience.Summary,
                        Methodologies = experience.Methodologies,
                        Transfer = experience.Transfer,
                        DataRegistration = experience.DataRegistration,
                        UserId = experience.UserId,
                        InstitutionId = experience.InstitutionId
                    });
                }

                return ExperiencieDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las experiencias");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de experiencias", ex);
            }
        }

        // Método para obtener una experiencia por ID como DTO
        public async Task<ExperiencieDTO> GetExperienceByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una experiencia con ID inválido: {ExperienceId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID de la experiencia debe ser mayor que cero");
            }

            try
            {
                var experiencie = await _experiencieData.GetByIdAsync(id);
                if (experiencie == null)
                {
                    _logger.LogInformation("No se encontró ninguna experiencia con ID: {ExperienceId}", id);
                    throw new EntityNotFoundException("Experience", id);
                }

                return new ExperiencieDTO
                {
                    Id = experiencie.Id,
                    NameExperience = experiencie.NameExperience,
                    Summary = experiencie.Summary,
                    Methodologies = experiencie.Methodologies,
                    Transfer = experiencie.Transfer,
                    DataRegistration = experiencie.DataRegistration,
                    UserId = experiencie.UserId,
                    InstitutionId = experiencie.InstitutionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la experiencia con ID: {ExperienceId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la experiencia con ID {id}", ex);
            }
        }

        // Método para crear una experiencia desde un DTO
        public async Task<ExperiencieDTO> CreateExperienceAsync(ExperiencieDTO ExperiencieDTO)
        {
            try
            {
                ValidateExperience(ExperiencieDTO);

                var experiencie = new Experiencie
                {

                    NameExperience = ExperiencieDTO.NameExperience,
                    Methodologies = ExperiencieDTO.Methodologies,
                    Summary = ExperiencieDTO.Summary,
                    Transfer = ExperiencieDTO.Transfer,
                    DataRegistration = ExperiencieDTO.DataRegistration,
                    UserId = ExperiencieDTO.UserId,
                    InstitutionId = ExperiencieDTO.InstitutionId
                };

                var experiencieCreated = await _experiencieData.CreateAsync(experiencie);

                return new ExperiencieDTO
                {
                    NameExperience = experiencieCreated.NameExperience,
                    Methodologies = experiencieCreated.Methodologies,
                    Summary = experiencieCreated.Summary,
                    Transfer = experiencieCreated.Transfer,
                    DataRegistration = experiencieCreated.DataRegistration,
                    UserId = experiencieCreated.UserId,
                    InstitutionId = experiencieCreated.InstitutionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva experiencia: {ExperienceTitle}", ExperiencieDTO?.NameExperience ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperience(ExperiencieDTO ExperiencieDTO)
        {
            if (ExperiencieDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(ExperiencieDTO.NameExperience))
            {
                _logger.LogWarning("Se intentó crear/actualizar una experiencia con Title vacío");
                throw new Utilities.Exeptions.ValidationException("Title", "El Title de la experiencia es obligatorio");
            }
        }
    }
}

  
