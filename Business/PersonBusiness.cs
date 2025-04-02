using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class PersonBusiness
    {
        private readonly PersonData _rolData;
        private readonly ILogger _logger;

        public PersonBusiness(PersonData PersonData, ILogger logger)
        {
            _PersonData = PersonData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<PersonDto>> GetAllRolesAsync()
        {
            try
            {
                var person = await _PersonData.GetAllAsync();
                var personDTO = new List<PersonDto>();

                foreach (var person in Person)
                {
                    personDTO.Add(new PersonDto
                    {
                        Id = person.Id,
                        Name = person.Name,
                        Active = person.Active // Si existe en la entidad
                    });
                }

                return personDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos las personas");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de personas", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<PersonDto> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un person con ID inválido: {personId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del person debe ser mayor que cero");
            }

            try
            {
                var person = await _rolData.GetByIdAsync(id);
                if (person == null)
                {
                    _logger.LogInformation("No se encontró ningún person con ID: {PersonId}", id);
                    throw new EntityNotFoundException("Person", id);
                }

                return new PersonDto
                {
                    Id = person.Id,
                    Name = person.Name,
                    Active = person.Active 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el person con ID: {personId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el person con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<PersonDto> CreateRolAsync(PersonDto PersonDto)
        {
            try
            {
                ValidateRol(PersonDto);

                var rol = new person
                    Name = PersonDto.Name,
                    Active = PersonDto.Active // Si existe en la entidad
                };

                var personCreado = await _personData.CreateAsync(person);

                return new PersonDto
                {
                    Id = personCreado.Id,
                    Name = personCreado.Name,
                    Active = personCreado.Active // Si existe en la entidad
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", PersonDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el person", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRol(PersonDto PersonDto)
        {
            if (PersonDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto person no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(PersonDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un person con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del person es obligatorio");
            }
        }
    }

