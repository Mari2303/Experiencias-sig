using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

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
        public async Task<IEnumerable<EvaluationDto>> GetAllEvaluationsAsync()
        {
            try
            {
                var evaluations = await _evaluationData.GetAllAsync();
                return evaluations.Select(eval => new EvaluationDto
                {
                    Id = eval.Id,
                    Name = eval.Name,
                    Description = eval.Description,
                    CreatedAt = eval.CreatedAt,
                    UpdatedAt = eval.UpdatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las evaluaciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de evaluaciones", ex);
            }
        }

        // Método para obtener una evaluación por ID como DTO
        public async Task<EvaluationDto> GetEvaluationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una evaluación con ID inválido: {EvaluationId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la evaluación debe ser mayor que cero");
            }

            try
            {
                var evaluation = await _evaluationData.GetByIdAsync(id);
                if (evaluation == null)
                {
                    _logger.LogInformation("No se encontró ninguna evaluación con ID: {EvaluationId}", id);
                    throw new EntityNotFoundException("Evaluation", id);
                }

                return new EvaluationDto
                {
                    Id = evaluation.Id,
                    Name = evaluation.Name,
                    Description = evaluation.Description,
                    CreatedAt = evaluation.CreatedAt,
                    UpdatedAt = evaluation.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la evaluación con ID: {EvaluationId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la evaluación con ID {id}", ex);
            }
        }

        // Método para crear una evaluación desde un DTO
        public async Task<EvaluationDto> CreateEvaluationAsync(EvaluationDto evaluationDto)
        {
            try
            {
                ValidateEvaluation(evaluationDto);

                var evaluation = new Evaluation
                {
                    Name = evaluationDto.Name,
                    Description = evaluationDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdEvaluation = await _evaluationData.CreateAsync(evaluation);

                return new EvaluationDto
                {
                    Id = createdEvaluation.Id,
                    Name = createdEvaluation.Name,
                    Description = createdEvaluation.Description,
                    CreatedAt = createdEvaluation.CreatedAt,
                    UpdatedAt = createdEvaluation.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva evaluación: {EvaluationName}", evaluationDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la evaluación", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateEvaluation(EvaluationDto evaluationDto)
        {
            if (evaluationDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto evaluación no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(evaluationDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una evaluación con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la evaluación es obligatorio");
            }
        }
    }
}
