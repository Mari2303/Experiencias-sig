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
    /// Clase de negocio encargada de la lógica relacionada con las experiencias en el sistema.
    /// </summary>
    public class ExperiencieBusiness
    {
        private readonly ExperiencieData _experiencieData;
        private readonly ILogger<Experiencie> _logger;

        public ExperiencieBusiness(ExperiencieData experiencieData, ILogger<Experiencie> logger)
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


                return MapToDTOList(experiencie);


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


                return MapToDTO(experiencie);





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


                var experiencie = MapToEntity(ExperiencieDTO);

                var experiencieCreate = await _experiencieData.CreateAsync(experiencie);

                return MapToDTO(experiencieCreate);



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



        // PATCH - actualizar campos individuales
        public async Task<bool> ExperiencieAsync(int id, string nameExperience, string summary, string methodologies, string transfer, string dataRegistration, int userId, int institutionId)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (string.IsNullOrWhiteSpace(nameExperience))
                throw new ValidationException("nameExperience", "El nombre de la experiencia es obligatorio.");

            var result = await _experiencieData.ExperiencieAsync(id, nameExperience, summary, methodologies, transfer, dataRegistration, userId, institutionId);

            if (!result)
                throw new EntityNotFoundException("Experiencie", id);

            return true;
        }

        // PUT - actualizar todos los campos usando DTO
        public async Task<bool> PutExperiencieAsync(int id, ExperiencieDTO dto)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (dto == null)
                throw new ValidationException("Experiencie", "Datos de experiencia inválidos.");

            var result = await _experiencieData.PutExperiencieAsync(id, dto.NameExperience, dto.Summary, dto.Methodologies, dto.Transfer, dto.DataRegistration, dto.UserId, dto.InstitutionId);

            if (!result)
                throw new EntityNotFoundException("Experiencie", id);

            return true;
        }













        // Método para mapear una entidad a un DTO


        private ExperiencieDTO MapToDTO(Experiencie experiencie)
        {
            return new ExperiencieDTO
            {
                Id = experiencie.Id,
                NameExperience = experiencie.NameExperience,
                Summary = experiencie.Summary,
                Methodologies = experiencie.Methodologies,
                Transfer = experiencie.Transfer,
                DataRegistration = experiencie.DataRegistration,
                UserId = experiencie.UserId,
                UserName = experiencie.UserName,
                InstitutionId = experiencie.InstitutionId,
                InstitutionName = experiencie.InstitutionName
            };
        }

        // Método para mapear un DTO a una entidad

        private Experiencie MapToEntity(ExperiencieDTO experiencieDTO)
        {
            return new Experiencie
            {
                Id = experiencieDTO.Id,
                NameExperience = experiencieDTO.NameExperience,
                Summary = experiencieDTO.Summary,
                Methodologies = experiencieDTO.Methodologies,
                Transfer = experiencieDTO.Transfer,
                DataRegistration = experiencieDTO.DataRegistration,
                UserId = experiencieDTO.UserId,
                InstitutionId = experiencieDTO.InstitutionId
            };
        }

        // Método para mapear una lista de entidades a una lista de DTOs
        private IEnumerable<ExperiencieDTO> MapToDTOList(IEnumerable<Experiencie> experiencies)
        {
            var experiencieDTOs = new List<ExperiencieDTO>();

            foreach (var experiencie in experiencies)
            {
                experiencieDTOs.Add(MapToDTO(experiencie));
            }
            return experiencieDTOs;




        }
    }
}

  
