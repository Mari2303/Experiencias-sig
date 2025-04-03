using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
  public class ExperiencieLineThematicData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public ExperiencieLineThematicData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los  almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista s</returns>

        public async Task<IEnumerable<ExperiencieLineThematic>> GetAllAsync()
        {
            return await _context.Set<ExperiencieLineThematic>().ToListAsync();
        }

        ///<summary> Obtiene  específico por su identificador.

        public async Task<ExperiencieLineThematic?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<ExperiencieLineThematic>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ExperiencieLineThematic con ID {ExperiencieLineThematicId}", id);
                throw;//Re-lanza la excepción para que sea manejada en capas superiores
            }
        }

        ///<summary>
        ///Crea un nuevo la bse de datos.
        ///</summary>
        ///<param name="ExperiencieLineThematic>Instancia del ExperiencieLineThematic a crear</param>
        ///<returns>El  creado</returns>

        public async Task<ExperiencieLineThematic> CreateAsync(ExperiencieLineThematic ExperiencieLineThematic)
        {
            try
            {
                await _context.Set<ExperiencieLineThematic>().AddAsync(ExperiencieLineThematic);
                await _context.SaveChangesAsync();
                return ExperiencieLineThematic;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el ExperiencieLineThematic:{ex.Message}");
                throw;
            }
        }

        ///<summary>
        ///Actualiza  existente en la base de datos.
        ///</summary>
        ///<param name="ExperiencieLineThematic">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(ExperiencieLineThematic experiencieLineThematic)
        {
            try
            {
                _context.Set<ExperiencieLineThematic>().Update(experiencieLineThematic);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el ExperiencieLineThematic: {ex.Message}");
                return false;
            }
        }

        ///<summary>
        ///Elimina  de la base de datos.
        ///</summary>
        ///<param name="id">Identificador único del  a eliminar</param>
        ///<returns>True si la eliminación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var ExperiencieLineThematic = await _context.Set<ExperiencieLineThematic>().FindAsync(id);
                if (ExperiencieLineThematic == null)
                    return false;

                _context.Set<ExperiencieLineThematic>().Remove(ExperiencieLineThematic);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                {
                    Console.WriteLine($"Error al eliminar el ExperiencieLineThematic: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
