using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las evaluaciones en el sistema.
    /// </summary>
    public class EvaluationBusiness
    {
        private readonly EvaluationData _evaluationData;
        private readonly ILogger _logger;

        public EvaluationBusiness(EvaluationData evaluationData, ILogger logger)
        {
            _evaluationData = evaluationData;
            _logger = logger;
        }

        // Método para obtener todas las evaluaciones como DTOs
        public async Task<IEnumerable<EvaluationDTO>> GetAllEvaluationsAsync()
        {
            try
            {
                var evaluations = await _evaluationData.GetAllAsync();
                
                 return MapToDTOList(evaluations);

                



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las evaluaciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de evaluaciones", ex);
            }
        }

        // Método para obtener una evaluación por ID como DTO
        public async Task<EvaluationDTO> GetEvaluationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una evaluación con ID inválido: {EvaluationId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID de la evaluación debe ser mayor que cero");
            }

            try
            {
                var evaluation = await _evaluationData.GetByIdAsync(id);
                if (evaluation == null)
                {
                    _logger.LogInformation("No se encontró ninguna evaluación con ID: {EvaluationId}", id);
                    throw new EntityNotFoundException("Evaluation", id);
                }



                return MapToDTO(evaluation);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la evaluación con ID: {EvaluationId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la evaluación con ID {id}", ex);
            }
        }

        // Método para crear una evaluación desde un DTO
        public async Task<EvaluationDTO> CreateEvaluationAsync(EvaluationDTO EvaluationDTO)
        {
            try
            {
                ValidateEvaluation(EvaluationDTO);


               var evaluation = MapToEntity(EvaluationDTO);

                var evaluationCreado = await _evaluationData.CreateAsync(evaluation);

                return MapToDTO(evaluationCreado);




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva evaluación: {EvaluationName}", EvaluationDTO?.TypeEvaluation ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la evaluación", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateEvaluation(EvaluationDTO EvaluationDTO)
        {
            if (EvaluationDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto evaluación no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(EvaluationDTO.TypeEvaluation))
            {
                _logger.LogWarning("Se intentó crear/actualizar una evaluación con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name de la evaluación es obligatorio");
            }
        }




        





        // Metodo para mapear de Evaluation a RolDTO 

        private EvaluationDTO MapToDTO(Evaluation evaluation)
        {
            return new EvaluationDTO
            {
                Id = evaluation.Id,
                TypeEvaluation = evaluation.TypeEvaluation,
                Comments = evaluation.Comments,
                DateTime = evaluation.DateTime,
                UserId = evaluation.UserId,
                ExperiencieId = evaluation.ExperiencieId,
                StateId = evaluation.StateId

            };
        }

        // Metodo para mapear de Document a Document 
        private Evaluation MapToEntity(EvaluationDTO evaluationDTO)
        {
            return new Evaluation
            {

                Id = evaluationDTO.Id,
                TypeEvaluation = evaluationDTO.TypeEvaluation,
                Comments = evaluationDTO.Comments,
                DateTime = evaluationDTO.DateTime,
                UserId = evaluationDTO.UserId,
                ExperiencieId = evaluationDTO.ExperiencieId,
                StateId = evaluationDTO.StateId

            };
        }

        // Metodo para mapear una lista de Document a una lista de  DocumentDTO

        private IEnumerable<EvaluationDTO>MapToDTOList(IEnumerable<Evaluation> evaluations)
        {
            var evaluationDTO = new List<EvaluationDTO>();

            foreach (var evaluation in evaluations)
            {
                evaluationDTO.Add(MapToDTO(evaluation));

            }
            return evaluationDTO;


        }








    }
}
