using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
 public   class LineThematicData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LineThematic> _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public LineThematicData(ApplicationDbContext context, ILogger<LineThematic> logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los roles almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista de roles</returns>

        public async Task<IEnumerable<LineThematic>> GetAllAsync()
        {                             
            return await _context.Set<LineThematic>().ToListAsync();
        }                             

        ///<summary> Obtiene un rol específico por su identificador.

        public async Task<LineThematic?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<LineThematic>().FindAsync(id);
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
        ///<param name="lineThematic">Instancia del rol a crear</param>
        ///<returns>El rol creado</returns>

        public async Task<LineThematic> CreateAsync(LineThematic lineThematic)
        {
            try
            {
                await _context.Set<LineThematic>().AddAsync(lineThematic);
                await _context.SaveChangesAsync();
                return lineThematic;
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
        ///<param name="lineThematic">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(LineThematic lineThematic)
        {
            try
            {
                _context.Set<LineThematic>().Update(lineThematic);
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



        // PATCH: Actualización parcial
        public async Task<bool> UpdatePartialAsync(int id, string name, bool active)
        {
            try
            {
                var line = await _context.LineThematic.FindAsync(id);
                if (line == null)
                    return false;

                line.Name = name;
                line.Active = active;

                _context.Entry(line).Property(x => x.Name).IsModified = true;
                _context.Entry(line).Property(x => x.Active).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al hacer PATCH de LineThematic con ID {id}");
                return false;
            }
        }

        // PUT: Actualización completa
        public async Task<bool> UpdateFullAsync(int id, string name, bool active)
        {
            try
            {
                var line = await _context.LineThematic.FindAsync(id);
                if (line == null)
                    return false;

                line.Name = name;
                line.Active = active;

                _context.Entry(line).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al hacer PUT de LineThematic con ID {id}");
                return false;
            }
        }

        // DELETE lógico
        public async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                var line = await _context.LineThematic.FindAsync(id);
                if (line == null)
                    return false;

                line.Active = false;
                _context.Entry(line).Property(x => x.Active).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar lógicamente LineThematic con ID {id}");
                return false;
            }
        }


    }
}
