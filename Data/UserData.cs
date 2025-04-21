using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
    public class UserData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<User> _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public UserData(ApplicationDbContext context, ILogger<User> logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los roles almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista de roles</returns>

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Set<User>().ToListAsync();
        }

        ///<summary> Obtiene un rol específico por su identificador.

        public async Task<User?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<User>().FindAsync(id);
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
        ///<param name="user">Instancia del rol a crear</param>
        ///<returns>El rol creado</returns>

        public async Task<User> CreateAsync(User user)
        {
            try
            {
                await _context.Set<User>().AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
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
        ///<param name="user">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(User user)
        {
            try
            {
                _context.Set<User>().Update(user);
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

        public async Task<bool> UpdatePartialAsync(int id, string name, string email, string password, bool active, int personId, string personName)
        {
            try
            {
                var user = await _context.User.FindAsync(id);
                if (user == null)
                    return false;

                user.Name = name;
                user.Email = email;
                user.Password = password;
                user.Active = active;
                user.PersonId = personId;
                user.PersonName = personName;

                _context.Entry(user).Property(u => u.Name).IsModified = true;
                _context.Entry(user).Property(u => u.Email).IsModified = true;
                _context.Entry(user).Property(u => u.Password).IsModified = true;
                _context.Entry(user).Property(u => u.Active).IsModified = true;
                _context.Entry(user).Property(u => u.PersonId).IsModified = true;
                _context.Entry(user).Property(u => u.PersonName).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al aplicar PATCH al usuario con ID {id}");
                return false;
            }
        }

        public async Task<bool> UpdateFullAsync(int id, string name, string email, string password, bool active, int personId, string personName)
        {
            try
            {
                var user = await _context.User.FindAsync(id);
                if (user == null)
                    return false;

                user.Name = name;
                user.Email = email;
                user.Password = password;
                user.Active = active;
                user.PersonId = personId;
                user.PersonName = personName;

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al aplicar PUT al usuario con ID {id}");
                return false;
            }
        }

        public async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                var user = await _context.User.FindAsync(id);
                if (user == null)
                    return false;

                user.Active = false;
                _context.Entry(user).Property(u => u.Active).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar lógicamente el usuario con ID {id}");
                return false;
            }
        }
    }
}