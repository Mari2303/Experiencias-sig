using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Utilities.Exeptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class PersonBusiness
    {
        private readonly PersonData _personData;
        private readonly ILogger<Person> _logger;

        public PersonBusiness(PersonData PersonData, ILogger<Person> logger)
        {
            _personData = PersonData;

            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<PersonDTO>> GetAllPersonAsync()
        {
            try
            {
                var person = await _personData.GetAllAsync();


                return MapToDTOList(person);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos las personas");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de personas", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<PersonDTO> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un person con ID inválido: {personId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del person debe ser mayor que cero");
            }

            try
            {
                var person = await _personData.GetByIdAsync(id);
                if (person == null)
                {
                    _logger.LogInformation("No se encontró ningún person con ID: {PersonId}", id);
                    throw new EntityNotFoundException("Person", id);
                }


                return MapToDTO(person);





            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el person con ID: {personId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el person con ID {id}", ex);
            }
        }

        // Método desde un DTO
        public async Task<PersonDTO> CreatePersonAsync(PersonDTO PersonDTO)
        {
            try
            {
                ValidatePerson(PersonDTO);

                var person = MapToEntity(PersonDTO);

                var persona = await _personData.CreateAsync(person);

                return MapToDTO(person);


            }
            catch (Exception ex)

            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", PersonDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el person", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePerson(PersonDTO PersonDTO)
        {
            if (PersonDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto  puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(PersonDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name no es obligatorio");
            }
        }


        // Metodo para mapear una entidad a DTO 

        private PersonDTO MapToDTO(Person person)
        {
            return new PersonDTO
            {
                Id = person.Id,
                Name = person.Name,
                Email = person.Email,
                Phone = person.Phone,
                Active = person.Active,
                UserId = person.UserId
            };
        }


        // Metodo para mapear un DTO a entidad 

        private Person MapToEntity(PersonDTO personDTO)
        {
            return new Person
            {
                Id = personDTO.Id,
                Name = personDTO.Name,
                Email = personDTO.Email,
                Phone = personDTO.Phone,
                Active = personDTO.Active,
                UserId = personDTO.UserId
            };
        }



        // Método para mapear una lista de entidades a DTOs

        private IEnumerable<PersonDTO> MapToDTOList(IEnumerable<Person> persons)
        {
            var personDTOs = new List<PersonDTO>();
            foreach (var person in persons)
            {
                personDTOs.Add(MapToDTO(person));
            }
            return personDTOs;





        }
    }

}

