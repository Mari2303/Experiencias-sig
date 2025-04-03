using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
  public  class EvaluationData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public EvaluationData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos  almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista </returns>

        public async Task<IEnumerable<Evaluation>> GetAllAsync()
        {
            return await _context.Set<Evaluation>().ToListAsync();
        }

        ///<summary> Obtiene específico por su identificador.

        public async Task<Evaluation?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Evaluation>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ron con ID {RolId}", id);
                throw;//Re-lanza la excepción para que sea manejada en capas superiores
            }
        }

        ///<summary>
        ///Crea un nuevo en la base de datos
        ///</summary>
        ///<param name="evaluation">Instancia del rol a crear</param>
        ///<returns>El  creado</returns>

        public async Task<Evaluation> CreateAsync(Evaluation evaluation)
        {
            try
            {
                await _context.Set<Evaluation>().AddAsync(evaluation);
                await _context.SaveChangesAsync();
                return evaluation;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el rol:{ex.Message}");
                throw;
            }
        }

        ///<summary>
        ///Actualiza existente en la base de datos.
        ///</summary>
        ///<param name="evaluation">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(Evaluation evaluation)
        {
            try
            {
                _context.Set<Evaluation>().Update(evaluation);
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
        ///Elimina  de la base de datos.
        ///</summary>
        ///<param name="id">Identificador único  eliminar</param>
        ///<returns>True si la eliminación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var evaluation = await _context.Set<Evaluation>().FindAsync(id);
                if (evaluation == null)
                    return false;

                _context.Set<Evaluation>().Remove(evaluation);
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
    }
}
