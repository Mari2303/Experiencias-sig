using Entity.Context;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
    public class HistoryExperienceData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HistoryExperience> _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public HistoryExperienceData(ApplicationDbContext context, ILogger<HistoryExperience> logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los roles almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista de roles</returns>

        public async Task<IEnumerable<HistoryExperience>> GetAllAsync()
        {
            return await _context.Set<HistoryExperience>().ToListAsync();
        }

        ///<summary> Obtiene un rol específico por su identificador.

        public async Task<HistoryExperience?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<HistoryExperience>().FindAsync(id);
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
        ///<param name="historyExperiencia">Instancia del rol a crear</param>
        ///<returns>El rol creado</returns>

        public async Task<HistoryExperience> CreateAsync(HistoryExperience historyExperience)
        {
            try
            {
                await _context.Set<HistoryExperience>().AddAsync(historyExperience);
                await _context.SaveChangesAsync();
                return historyExperience;
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
        ///<param name="historyExperience">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(HistoryExperience historyExperience)
        {
            try
            {
                _context.Set<HistoryExperience>().Update(historyExperience);
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

        public async Task<bool?> PutHistoryExperienceAsync(int id, HistoryExperienceDTO dto)
        {
            var entity = await _context.HistoryExperience.FindAsync(id);
            if (entity == null) return null;

            entity.Action = dto.Action;
            entity.DateTime = dto.DateTime;
            entity.TableName = dto.TableName;
            entity.ExperiencieId = dto.ExperiencieId;
            entity.UserId = dto.UserId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool?> PatchHistoryExperienceAsync(int id, bool action, DateTime dateTime, string tableName, int experiencieId, int userId)
        {
            var entity = await _context.HistoryExperience.FindAsync(id);
            if (entity == null) return null;

            entity.Action = action;
            entity.DateTime = dateTime;
            entity.TableName = tableName;
            entity.ExperiencieId = experiencieId;
            entity.UserId = userId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool?> DeleteHistoryExperienceAsync(int id)
        {
            var entity = await _context.HistoryExperience.FindAsync(id);
            if (entity == null) return null;

            entity.Action = false;
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
