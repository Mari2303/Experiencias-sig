using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las verificaciones en el sistema.
    /// </summary>
    public class VerificationBusiness
    {
        private readonly VerificationData _verificationData;
        private readonly ILogger _logger;

        public VerificationBusiness(VerificationData verificationData, ILogger logger)
        {
            _verificationData = verificationData;
            _logger = logger;
        }

        // Método para obtener todas las verificaciones como DTOs
        public async Task<IEnumerable<VerificationDto>> GetAllVerificationsAsync()
        {
            try
            {
                var verifications = await _verificationData.GetAllAsync();
                return verifications.Select(verification => new VerificationDto
                {
                    Id = verification.Id,
                    Name = verification.Name,
                    Description = verification.Description,
                    CreatedAt = verification.CreatedAt,
                    UpdatedAt = verification.UpdatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las verificaciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de verificaciones", ex);
            }
        }

        // Método para obtener una verificación por ID como DTO
        public async Task<VerificationDto> GetVerificationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una verificación con ID inválido: {VerificationId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la verificación debe ser mayor que cero");
            }

            try
            {
                var verification = await _verificationData.GetByIdAsync(id);
                if (verification == null)
                {
                    _logger.LogInformation("No se encontró ninguna verificación con ID: {VerificationId}", id);
                    throw new EntityNotFoundException("Verification", id);
                }

                return new VerificationDto
                {
                    Id = verification.Id,
                    Name = verification.Name,
                    Description = verification.Description,
                    CreatedAt = verification.CreatedAt,
                    UpdatedAt = verification.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la verificación con ID: {VerificationId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la verificación con ID {id}", ex);
            }
        }

        // Método para crear una verificación desde un DTO
        public async Task<VerificationDto> CreateVerificationAsync(VerificationDto verificationDto)
        {
            try
            {
                ValidateVerification(verificationDto);

                var verification = new Verification
                {
                    Name = verificationDto.Name,
                    Description = verificationDto.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var createdVerification = await _verificationData.CreateAsync(verification);

                return new VerificationDto
                {
                    Id = createdVerification.Id,
                    Name = createdVerification.Name,
                    Description = createdVerification.Description,
                    CreatedAt = createdVerification.CreatedAt,
                    UpdatedAt = createdVerification.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva verificación: {VerificationName}", verificationDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la verificación", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateVerification(VerificationDto verificationDto)
        {
            if (verificationDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto verificación no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(verificationDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una verificación con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la verificación es obligatorio");
            }
        }
    }
}
