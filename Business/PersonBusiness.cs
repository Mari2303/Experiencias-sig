using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Utilities.Exeptions;
using ValidationException = Utilities.Exeptions.ValidationException;

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



        public async Task<bool> PutPersonAsync(int id, PersonDTO dto)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");
            if (dto == null)
                throw new ValidationException("Person", "Datos inválidos.");

            return await _personData.PutPersonAsync(id, dto.Name, dto.Email, dto.Phone, dto.Surname, dto.Document, dto.codeDane, dto.Password, dto.Active)
                ?? throw new EntityNotFoundException("Person", id);
        }




        public async Task<bool> PatchPersonAsync(int id,  string name, string Surname, string Document, string email, string phone, string Password, string password, bool active)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            return await _personData.PatchPersonAsync(id, name, email, Surname, Document, phone,Password, active)
                ?? throw new EntityNotFoundException("Person", id);
        }

        public async Task<bool> DeletePersonAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            return await _personData.DeletePersonAsync(id)
                ?? throw new EntityNotFoundException("Person", id);
        }
        /// <summary>
        /// Elimina permanentemente una persona y sus relaciones asociadas por su ID.
        /// </summary>
        /// <param name="id">ID de la persona a eliminar.</param>
        /// <returns>Objeto PersonDto de la persona eliminada.</returns>
        /// <exception cref="ValidationException">Si el ID es inválido.</exception>
        /// <exception cref="EntityNotFoundException">Si no se encuentra la persona.</exception>
        /// <exception cref="ExternalServiceException">Si ocurre un error al eliminar la persona.</exception>
        public async Task<PersonDTO> DeletePermanentAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó eliminar permanentemente una persona con ID inválido: {PersonId}", id);
                throw new ValidationException("id", "El ID debe ser mayor que cero");
            }

            try
            {
                var person = await _personData.GetByIdAsync(id);
                if (person == null)
                {
                    _logger.LogInformation("No se encontró ninguna persona con ID: {PersonId}", id);
                    throw new EntityNotFoundException("Person", id);
                }

                var result = await _personData.DeleteAsync(id);
                if (result != true)
                {
                    throw new ExternalServiceException("Base de datos", $"No se pudo eliminar la persona con ID {id}");
                }

                return MapToDTO(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente la persona con ID: {PersonId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al eliminar permanentemente la persona con ID {id}", ex);
            }
        }



        public async Task<Person> CreatePersonWithUserAsync(PersonDTO personDTO)
        {
            return await _personData.CreateWithUserAsync(personDTO);
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
                Surname = person.Surname,
                Document = person.Document,
                codeDane = person.codeDane,
                Password = person.Password,
                Active = person.Active

                
           
            };
        }


        // Metodo para mapear un DTO a entidad 

        private Person MapToEntity(PersonDTO personDTO)
        {
            return new Person
            {
                Id = personDTO.Id,
                Name = personDTO.Name,
                Surname = personDTO.Surname,
                Document = personDTO.Document,
                codeDane = personDTO.codeDane,
                Password = personDTO.Password,
                Email = personDTO.Email,
                Phone = personDTO.Phone,
                Active = personDTO.Active,
               
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

