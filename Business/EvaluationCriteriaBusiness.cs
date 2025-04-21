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
    /// Clase de negocio encargada de la lógica relacionada  del sistema.
    /// </summary>
    public class EvaluationCriteriaBusiness
    {
        private readonly EvaluationCriteriaData _EvaluationCriteriaData;
        private readonly ILogger<EvaluationCriteria> _logger;

        public EvaluationCriteriaBusiness(EvaluationCriteriaData evaluationCriteriaData, ILogger<EvaluationCriteria> logger)
        {
            _EvaluationCriteriaData = evaluationCriteriaData;
            _logger = logger;
        }

        // Método para obtener  los DTOs
        public async Task<IEnumerable<EvaluationCriteriaDTO>> GetAllEvaluationCriteriaAsync()
        {
            try
            {
                var evaluationCri = await _EvaluationCriteriaData.GetAllAsync();
               
                return MapToDTOList(evaluationCri);

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todo");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista ", ex);
            }
        }

        // Método para obtener un  ID como DTO
        public async Task<EvaluationCriteriaDTO> GetEvaluationCriteriaByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un  ID inválido: {EvaluationCriteriaId}", id);
                throw new ValidationException("id", "El ID del EvaluationCriteria debe ser mayor que cero");
            }

            try
            {
                var evaluationCriteria = await _EvaluationCriteriaData.GetByIdAsync(id);
                if (evaluationCriteria == null)
                {
                    _logger.LogInformation("No se encontró ningún EvaluationCriteria con ID: {EvaluationCriteriaId}", id);
                    throw new EntityNotFoundException("EvaluationCriteria", id);
                }



                return MapToDTO(evaluationCriteria);


                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el  ID: {EvaluationCriteriaId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el EvaluationCriteria con ID {id}", ex);
            }
        }

        // Método para crear  un DTO
        public async Task<EvaluationCriteriaDTO> CreateEvaluationCriteriaAsync(EvaluationCriteriaDTO EvaluationCriteriaDTO)
        {
            try
            {


                ValidateEvaluationCriteria(EvaluationCriteriaDTO);

                var evaluationCriteria = MapToEntity(EvaluationCriteriaDTO);

                var evaluationCriteriaCreated = await _EvaluationCriteriaData.CreateAsync(evaluationCriteria);

                return MapToDTO(evaluationCriteriaCreated);

            }
             


            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo : {EvaluationCririaNombre}", EvaluationCriteriaDTO?.Score ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear ", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateEvaluationCriteria(EvaluationCriteriaDTO evaluationCriteriaDTO )
        {
            if (evaluationCriteriaDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(  evaluationCriteriaDTO.Score))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }


        public async Task<bool> PutEvaluationCriteriaAsync(int id, EvaluationCriteriaDTO dto)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (dto == null)
                throw new ValidationException("dto", "Datos inválidos.");

            var result = await _EvaluationCriteriaData.PutEvaluationCriteriaAsync(id, dto.Score, dto.EvaluationId, dto.CriteriaId);
            if (!result)
                throw new EntityNotFoundException("EvaluationCriteria", id);

            return true;
        }

        public async Task<bool> PatchEvaluationCriteriaAsync(int id, string score, int evaluationId, int criteriaId)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            var result = await _EvaluationCriteriaData.PatchEvaluationCriteriaAsync(id, score, evaluationId, criteriaId);
            if (!result)
                throw new EntityNotFoundException("EvaluationCriteria", id);

            return true;
        }





        //Metodo para mapear de EvaluationCriteria a EvaluationCriteriaDTO

        private EvaluationCriteriaDTO MapToDTO(EvaluationCriteria evaluationCriteria)
        {

            return new EvaluationCriteriaDTO
            {
                Id = evaluationCriteria.Id,
                Score = evaluationCriteria.Score,
                CriteriaId = evaluationCriteria.CriteriaId,
                CriteriaName = evaluationCriteria.Criteria.Name,
                EvaluationId = evaluationCriteria.EvaluationId,
                EvaluationName = evaluationCriteria.EvaluationName

            };
        }


        // Metodo para maper de EvaluationDTO a Evaluation

        private EvaluationCriteria MapToEntity(EvaluationCriteriaDTO evaluationCriteriaDTO)
        {

            return new EvaluationCriteria
            {

                Id = evaluationCriteriaDTO.Id,
                Score = evaluationCriteriaDTO.Score,
                CriteriaId = evaluationCriteriaDTO.CriteriaId,
                EvaluationId = evaluationCriteriaDTO.EvaluationId
            };
        }

        //Metodo para mapear una lista de EvaluationCriteria a una lista de EvaluationCriteriaDTO

        private IEnumerable<EvaluationCriteriaDTO> MapToDTOList(IEnumerable<EvaluationCriteria> evaluationCri)
        {
            var evaluationCriDTO = new List<EvaluationCriteriaDTO>();

            foreach (var evaluationCriteria in evaluationCri)
            {
                evaluationCriDTO.Add(MapToDTO(evaluationCriteria));
            }

            return evaluationCriDTO;
        }
    }
}

