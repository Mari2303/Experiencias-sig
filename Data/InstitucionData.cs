using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
 public   class InstitucionData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Institucion> _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public      InstitucionData(ApplicationDbContext context, ILogger<Institucion> logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los roles almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista de roles</returns>

        public async Task<IEnumerable<Institucion>> GetAllAsync()
        {
            return await _context.Set<Institucion>().ToListAsync();
        }

        ///<summary> Obtiene un rol específico por su identificador.

        public async Task<Institucion?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Institucion>().FindAsync(id);
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
        ///<param name="Institucion">Instancia del rol a crear</param>
        ///<returns>El rol creado</returns>

        public async Task<Institucion> CreateAsync(Institucion Institucion)
        {
            try
            {
                await _context.Set<Institucion>().AddAsync(Institucion);
                await _context.SaveChangesAsync();
                return Institucion;
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
        ///<param name="Institucion">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(Institucion Institucion)
        {
            try
            {
                _context.Set<Institucion>().Update(Institucion);
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
        public async Task<bool> UpdatePartialAsync(int id, InstitucionDTO dto)
        {
            try
            {
                var institution = await _context.Institution.FindAsync(id);
                if (institution == null)
                    return false;

                institution.Name = dto.Name;
                institution.Address = dto.Address;
                institution.Phone = dto.Phone;
                institution.EmailInstitution = dto.EmailInstitution;
                institution.Department = dto.Department;
                institution.Commune = dto.Commune;
                institution.Active = dto.Active;


                _context.Entry(institution).Property(x => x.Name).IsModified = true;
                _context.Entry(institution).Property(x => x.Address).IsModified = true;
                _context.Entry(institution).Property(x => x.Phone).IsModified = true;
                _context.Entry(institution).Property(x => x.EmailInstitution).IsModified = true;
                _context.Entry(institution).Property(x => x.Department).IsModified = true;
                _context.Entry(institution).Property(x => x.Commune).IsModified = true;
                _context.Entry(institution).Property(x => x.Active).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PATCH de Institution con ID {id}");
                return false;
            }
        }

        // PUT: Actualización completa
        public async Task<bool> UpdateFullAsync(int id, InstitucionDTO dto)
        {
            try
            {
                var institution = await _context.Institution.FindAsync(id);
                if (institution == null)
                    return false;

                institution.Name = dto.Name;
                institution.Address = dto.Address;
                institution.Phone = dto.Phone;
                institution.EmailInstitution = dto.EmailInstitution;
                institution.Department = dto.Department;
                institution.Commune = dto.Commune;
                institution.Active = dto.Active;

                _context.Entry(institution).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PUT de Institution con ID {id}");
                return false;
            }
        }

        // Eliminación lógica
        public async Task<bool> DeleteLogicalAsync(int id)
        {
            try
            {
                var institution = await _context.Institution.FindAsync(id);
                if (institution == null)
                    return false;

                // Asegúrate de que la entidad Institution tenga el campo "Active"
                institution.Active = false;
                _context.Entry(institution).Property(x => x.Active).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar lógicamente la Institution con ID {id}");
                return false;
            }
        }











    }
}
