using Entity.Context;
using Entity.Model;
using Entity.ModelExperience;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
  public  class RolFromPermissionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolFromPermission> _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public RolFromPermissionData(ApplicationDbContext context, ILogger<RolFromPermission> logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los roles almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista de roles</returns>

        public async Task<IEnumerable<RolFromPermission>> GetAllAsync()
        {
            return await _context.Set<RolFromPermission>().ToListAsync();
        }

        ///<summary> Obtiene un rol específico por su identificador.

        public async Task<RolFromPermission?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<RolFromPermission>().FindAsync(id);
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
        ///<param name="rolPermission">Instancia del rol a crear</param>
        ///<returns>El rol creado</returns>

        public async Task<RolFromPermission> CreateAsync(RolFromPermission rolPermission)
        {
            try
            {
                await _context.Set<RolFromPermission>().AddAsync(rolPermission);
                await _context.SaveChangesAsync();
                return rolPermission;
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
        ///<param name="rolPermission">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(RolFromPermission rolPermission)
        {
            try
            {
                _context.Set<RolFromPermission>().Update(rolPermission);
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
                var rolPermission = await _context.Set<RolFromPermission>().FindAsync(id);
                if (rolPermission == null)
                    return false;

                _context.Set<RolFromPermission>().Remove(rolPermission);
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





        public async Task<bool> PatchRolPermissionAsync(int id, int rolId, int permissionId, int fromId)
        {
            var entity = await _context.RolPermissions.FindAsync(id);
            if (entity == null)
                return false;

            entity.RolId = rolId;
            entity.PermissionId = permissionId;

            _context.Entry(entity).Property(x => x.RolId).IsModified = true;
            _context.Entry(entity).Property(x => x.PermissionId).IsModified = true;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PutRolPermissionAsync(int id, int rolId, int permissionId, int fromId)
        {
            var entity = await _context.RolPermissions.FindAsync(id);
            if (entity == null)
                return false;

            entity.RolId = rolId;
            entity.PermissionId = permissionId;

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }


        /// <summary>
        /// Elimina permanentemente un rol-permiso de la base de datos.
        /// </summary>
        /// <param name="id">Identificador único del rol-permiso a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> DeletePermanentAsync(int id)
        {
            try
            {
                var entity = await _context.Set<RolFromPermission>().FindAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("No se encontró RolPermission con ID {RolPermissionId} para eliminar.", id);
                    return false;
                }

                _context.Set<RolFromPermission>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar permanentemente RolPermission con ID {RolPermissionId}", id);
                return false;
            }
        }







        // Add this method to the RolPermissionData class
        public async Task CreateIfNotExistsAsync(RolFromPermission rolPermission)
        {
            var existing = await _context.RolPermissions
                .FirstOrDefaultAsync(rp => rp.RolId == rolPermission.RolId && rp.PermissionId == rolPermission.PermissionId);

            if (existing == null)
            {
                await _context.RolPermissions.AddAsync(rolPermission);
                await _context.SaveChangesAsync();
            }
        }









    }
}
