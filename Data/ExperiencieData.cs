using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
public    class ExperiencieData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Experiencie> _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public ExperiencieData(ApplicationDbContext context, ILogger<Experiencie> logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los roles almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista de roles</returns>

        public async Task<IEnumerable<Experiencie>> GetAllAsync()
        {
            return await _context.Set<Experiencie>().ToListAsync();
        }

        ///<summary> Obtiene un rol específico por su identificador.

        public async Task<Experiencie?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Experiencie>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ron con ID {ExperiencieId}", id);
                throw;//Re-lanza la excepción para que sea manejada en capas superiores
            }
        }

        ///<summary>
        ///Crea un nuevo rol en la base de datos.
        ///</summary>
        ///<param name="Experiencie">Instancia del rol a crear</param>
        ///<returns>El rol creado</returns>

        public async Task<Experiencie> CreateAsync(Experiencie Experiencie)
        {
            try
            {
                await _context.Set<Experiencie>().AddAsync(Experiencie);
                await _context.SaveChangesAsync();
                return Experiencie;
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
        ///<param name="Experiencie">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(Experiencie Experiencie)
        {
            try
            {
                _context.Set<Experiencie>().Update(Experiencie);
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

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var Experiencie = await _context.Set<Experiencie>().FindAsync(id);
                if (Experiencie == null)
                    return false;

                _context.Set<Experiencie>().Remove(Experiencie);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                {
                    Console.WriteLine($"Error al eliminar el rol: {ex.Message}");
                    return false;
                }
            }
        }



        // PATCH - solo modifica campos específicos
        public async Task<bool> ExperiencieAsync(int id, string nameExperience, string summary, string methodologies, string transfer, string dataRegistration, int userId, int institutionId)
        {
            try
            {
                var experiencie = await _context.Experience.FindAsync(id);
                if (experiencie == null)
                    return false;

                experiencie.NameExperience = nameExperience;
                experiencie.Summary = summary;
                experiencie.Methodologies = methodologies;
                experiencie.Transfer = transfer;
                experiencie.DataRegistration = dataRegistration;
                experiencie.UserId = userId;
                experiencie.InstitutionId = institutionId;

                _context.Entry(experiencie).Property(e => e.NameExperience).IsModified = true;
                _context.Entry(experiencie).Property(e => e.Summary).IsModified = true;
                _context.Entry(experiencie).Property(e => e.Methodologies).IsModified = true;
                _context.Entry(experiencie).Property(e => e.Transfer).IsModified = true;
                _context.Entry(experiencie).Property(e => e.DataRegistration).IsModified = true;
                _context.Entry(experiencie).Property(e => e.UserId).IsModified = true;
                _context.Entry(experiencie).Property(e => e.InstitutionId).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al aplicar patch a la experiencia con ID {id}");
                return false;
            }
        }

        // PUT - reemplaza todos los campos
        public async Task<bool> PutExperiencieAsync(int id, string nameExperience, string summary, string methodologies, string transfer, string dataRegistration, int userId, int institutionId)
        {
            try
            {
                var experiencie = await _context.Experience.FindAsync(id);
                if (experiencie == null)
                    return false;

                experiencie.NameExperience = nameExperience;
                experiencie.Summary = summary;
                experiencie.Methodologies = methodologies;
                experiencie.Transfer = transfer;
                experiencie.DataRegistration = dataRegistration;
                experiencie.UserId = userId;
                experiencie.InstitutionId = institutionId;

                _context.Entry(experiencie).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar la experiencia con ID {id}");
                return false;
            }
        }














    }
}
