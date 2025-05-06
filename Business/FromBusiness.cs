﻿using Data;
using Entity.DTOs.Form;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;
using ValidationException = Utilities.Exeptions.ValidationException;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los formularios en el sistema.
    /// </summary>
    public class FromBusiness
    {
        private readonly FromData _formData;
        private readonly ILogger<FromData> _logger;

        public FromBusiness(FromData formData, ILogger<FromData> logger)
        {
            _formData = formData;
            _logger = logger;
        }

        // Método para obtener todos los formularios como DTOs
        public async Task<IEnumerable<FormDTO>> GetAllFormsAsync()
        {
            try
            {
                var forms = await _formData.GetAllAsync();

                return MapToDTOList(forms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los formularios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de formularios", ex);
            }
        }

        // Método para obtener un formulario por ID como DTO
        public async Task<FormDTO> GetFormByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un formulario con ID inválido: {FormId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del formulario debe ser mayor que cero");
            }

            try
            {
                var form = await _formData.GetByidAsync(id);
                if (form == null)
                {
                    _logger.LogInformation("No se encontró ningún formulario con ID: {FormId}", id);
                    throw new EntityNotFoundException("Form", id);
                }

                return MapToDTO(form);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formulario con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el formulario con ID {id}", ex);
            }
        }

        // Método para crear un formulario desde un DTO
        public async Task<FormDTO> CreateFormAsync(FormDTO formDto)
        {
            try
            {
                ValidateForm(formDto);

                var form = MapToEntity(formDto);
                form.CreateDate = DateTime.Now;

                var formCreado = await _formData.CreateAsync(form);

                return MapToDTO(formCreado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo formulario: {Name}", formDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el formulario", ex);
            }
        }

        public async Task<bool> SetFormActiveAsync(FormDTO dto)
        {
            if (dto == null)
                throw new ValidationException("El DTO de estado de formulario no puede ser nulo");

            if (dto.Id <= 0)
            {
                _logger.LogWarning("ID inválido para cambiar estado activo de formulario: {Id}", dto.Id);
                throw new ValidationException("Id", "El ID del formulario debe ser mayor a 0");
            }

            try
            {
                var entity = await _formData.GetByidAsync(dto.Id);
                if (entity == null)
                {
                    _logger.LogInformation("Formulario no encontrado con ID {Id} para cambiar estado", dto.Id);
                    throw new EntityNotFoundException("Form", dto.Id);
                }

                // Establecer DeleteDate si se va a desactivar (borrado lógico)
                if (!dto.Active)
                {
                    entity.DeleteDate = DateTime.Now;
                }
                else
                {
                    entity.DeleteDate = null; // Reactivación: eliminamos la marca de eliminación
                }

                return await _formData.SetActiveAsync(dto.Id, dto.Active);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado activo de formulario con ID {Id}", dto.Id);
                throw new ExternalServiceException("Base de datos", $"Error al actualizar estado activo de formulario con ID {dto.Id}", ex);
            }
        }

        public async Task<bool> DeleteFormAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar un formulario con ID inválido: {Id}", id);
                throw new ValidationException("Id", "El ID debe ser mayor a 0");
            }

            try
            {
                var exists = await _formData.GetByidAsync(id);
                if (exists == null)
                {
                    _logger.LogInformation("Formulario no encontrado con ID {Id} para eliminar", id);
                    throw new EntityNotFoundException("Form", id);
                }

                return await _formData.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar formulario con ID {Id}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar formulario con ID {id}", ex);
            }
        }

        public async Task<bool> UpdateParcialFormAsync(FormDTO dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                _logger.LogWarning("DTO de actualización parcial inválido");
                throw new ValidationException("Id", "Datos inválidos para actualizar formulario");
            }

            try
            {
                var exists = await _formData.GetByidAsync(dto.Id);
                if (exists == null)
                {
                    _logger.LogInformation("Formulario no encontrado con ID: {Id}", dto.Id);
                    throw new EntityNotFoundException("Form", dto.Id);
                }

                return await _formData.PatchAsync(dto.Id, dto.Name, dto.Description);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar parcialmente el formulario con ID {Id}", dto.Id);
                throw new ExternalServiceException("Base de datos", $"Error al actualizar formulario con ID {dto.Id}", ex);
            }
        }

        public async Task<bool> UpdateFormAsync(FormDTO dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                _logger.LogWarning("DTO de actualización inválido");
                throw new Utilities.Exeptions.ValidationException("id", "Datos inválidos para actualizar formulario");
            }

            try
            {
                var entity = await _formData.GetByidAsync(dto.Id);
                if (entity == null)
                    throw new EntityNotFoundException("Form", dto.Id);

                // Modifica sus campos directamente
                entity.Name = dto.Name;
                entity.Description = dto.Description;
                entity.UpdateDate = DateTime.Now;

                return await _formData.UpdateAsync(entity); //actualizas la misma instancia rastreada
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el formulario con ID {Id}", dto.Id);
                throw new ExternalServiceException("Base de datos", $"Error al actualizar formulario con ID {dto.Id}", ex);
            }
        }



        // Método para validar el DTO
        private void ValidateForm(FormDTO formDto)
        {
            if (formDto == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto formulario no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(formDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un formulario con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del formulario es obligatorio");
            }
        }

        //Metodo para mapear de Form a  FormDto
        private FormDTO MapToDTO(From form)
        {
            return new FormDTO
            {
                Id = form.Id,
                Name = form.Name,
                Description = form.Description,
                Active = form.Active,

            };
        }

        //Metodo para mapear de FormDto a Form 
        private From MapToEntity(FormDTO formDto)
        {
            return new From
            {
                Id = formDto.Id,
                Name = formDto.Name,
                Description = formDto.Description,
                Active = formDto.Active,

            };
        }
        //Metodo para mapear una lista de FormDto a una lista de Form
        private IEnumerable<FormDTO> MapToDTOList(IEnumerable<From> forms)
        {
            var formsDto = new List<FormDTO>();
            foreach (var form in forms)
            {
                formsDto.Add(MapToDTO(form));
            }
            return formsDto;
        }
    }
}