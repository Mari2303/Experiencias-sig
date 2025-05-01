using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
    public class PersonData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Person> _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public PersonData(ApplicationDbContext context, ILogger<Person> logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los roles almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista de roles</returns>

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.Set<Person>().ToListAsync();
        }

        ///<summary> Obtiene un rol específico por su identificador.

        public async Task<Person?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Person>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ron con ID {RolId}", id);
                throw;//Re-lanza la excepción para que sea manejada en capas superiores
            }
        }

        ///<summary>
        ///Crea un nuevo rol en la base de datos.
        ///</summary>
        ///<param name="person">Instancia del rol a crear</param>
        ///<returns>El rol creado</returns>

        public async Task<Person> CreateAsync(Person person)
        {
            try
            {
                await _context.Set<Person>().AddAsync(person);
                await _context.SaveChangesAsync();
                return person;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el rol:{ex.Message}");
                throw;
            }
        }

        ///<summary>
        ///Actualiza un rol existente en la base de datos.
        ///</summary>
        ///<param name="person">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(Person person)
        {
            try
            {
                _context.Set<Person>().Update(person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el rol: {ex.Message}");
                return false;
            }
        }

        ///<summary>
        ///Elimina un rol de la base de datos.
        ///</summary>
        ///<param name="id">Identificador único del rol a eliminar</param>
        ///<returns>True si la eliminación fue exitosa, False en caso contrario.</returns>

        public async Task<bool?> PutPersonAsync(int id, string name, string Surname, string Document, string email, string phone, string codeDane, string Password, bool active)
        {
            var entity = await _context.Person.FindAsync(id);
            if (entity == null) return null;

            entity.Name = name;
            entity.Email = email;
            entity.Phone = phone;
            entity.Surname = Surname;
            entity.Document = Document;
            entity.codeDane = codeDane;
            entity.Password = Password;
            entity.Active = active;
           

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool?> PatchPersonAsync(int id, string name, string Surname, string Document, string email, string phone, string Password, bool active)
        {
            var entity = await _context.Person.FindAsync(id);
            if (entity == null) return null;

            entity.Name = name;
            entity.Email = email;
            entity.Phone = phone;
            entity.Surname = Surname;
            entity.Document = Document;
            entity.Password = Password;
            entity.Active = active;
           

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool?> DeletePersonAsync(int id)
        {
            var entity = await _context.Person.FindAsync(id);
            if (entity == null) return null;

            entity.Active = false;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool?> DeleteAsync(int id)
        {
            try
            {
                var person = await _context.Person
                    .Include(p => p.User)   // Incluye User si hay relación

                    // .Include(p => p.OtraEntidadRelacionada) // Añade más si es necesario
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (person == null)
                    return null;

                // Elimina relaciones si existen
                if (person.User != null)
                    _context.User.Remove(person.User);

                // Elimina la persona
                _context.Person.Remove(person);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente la persona con ID {PersonId}", id);
                return false;
            }
        }








        public async Task<Person> CreateWithUserAsync(PersonDTO personDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var person = new Person
                {
                    Name = personDTO.Name,
                    Surname = personDTO.Surname,
                    Document = personDTO.Document,
                    Email = personDTO.Email,
                    Phone = personDTO.Phone,
                    codeDane = personDTO.codeDane,
                    Password = personDTO.Password, // en producción deberías encriptarla
                    Active = true,
                    CreateAt = DateTime.UtcNow
                };

                await _context.Person.AddAsync(person);
                await _context.SaveChangesAsync();

                var user = new User
                {
                    Email = person.Email,
                    Password = person.Password, // en producción: encripta
                    Person = person,
                    Active = true
                };

                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return person;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error al crear person con user");
                throw;
            }
        }




















    }
}







    





