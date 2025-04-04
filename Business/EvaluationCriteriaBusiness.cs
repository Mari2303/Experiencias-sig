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
        private readonly ILogger _logger;

        public EvaluationCriteriaBusiness(EvaluationCriteriaData evaluationCriteriaData, ILogger logger)
        {
            _EvaluationCriteriaData = evaluationCriteriaData;
            _logger = logger;
        }

        // Método para obtener  los DTOs
        public async Task<IEnumerable<EvaluationCriteriaDTO>> GetAllEvaluationCriteriaAsync(EvaluationCriteria evaluationCriteria)
        {
            try
            {
                var evaluationCri = await _EvaluationCriteriaData.GetAllAsync();
                var evaluationCriDTO = new List<EvaluationCriteriaDTO>();

                foreach (var EvaluationCriteria in evaluationCri)
                {
                    evaluationCriDTO.Add(new EvaluationCriteriaDTO
                    {
                        Id = evaluationCriteria.Id,
                        Score = evaluationCriteria.Score,
                        CriteriaId = evaluationCriteria.CriteriaId,
                        EvaluationId = evaluationCriteria.EvaluationId



                    });
                }

                return evaluationCriDTO;
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

                return new EvaluationCriteriaDTO
                {
                    Id = evaluationCriteria.Id,
                    Score = evaluationCriteria.Score,
                    CriteriaId = evaluationCriteria.CriteriaId,
                    EvaluationId = evaluationCriteria.EvaluationId
                };
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
                ValidateEvaluationCriteria(EvaluationCriteriaDTO, EvaluationCriteriaDTO);

                var evaluationCriteria = new EvaluationCriteria
                {

                    Score = EvaluationCriteriaDTO.Score,
                    CriteriaId = EvaluationCriteriaDTO.CriteriaId,
                    EvaluationId = EvaluationCriteriaDTO.EvaluationId
                    // Si existe en la entidad
                };

                var EvaluationCririaCreado = await _EvaluationCriteriaData.CreateAsync(evaluationCriteria);

                return new EvaluationCriteriaDTO
                {
                    Id = EvaluationCririaCreado.Id,
                    Score = EvaluationCririaCreado.Score,
                    CriteriaId = EvaluationCririaCreado.CriteriaId,
                    EvaluationId = EvaluationCririaCreado.EvaluationId // Si existe en la entidad
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo : {EvaluationCririaNombre}", EvaluationCriteriaDTO?.Score ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear ", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateEvaluationCriteria(EvaluationCriteriaDTO EvaluationCriteriaDTO, EvaluationCriteriaDTO evaluationCriteriaDTO)
        {
            if (evaluationCriteriaDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(EvaluationCriteriaDTO.Score))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }
    }
}

