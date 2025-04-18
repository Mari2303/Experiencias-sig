﻿
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    /// <summary>
    /// Repositorio encargado de la gestión de la entidad Rol en la base de datos.
    /// </summary>
    public class RolData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Rol> _logger;

        ///<summary>
        ///Constructor que recibe el contexto de base de datos.
        ///</summary>
        ///<param name="context">Instancia de <see cref="ApplicationDbContext"/>para la conexión con la base de datos.</param>

        public RolData(ApplicationDbContext context, ILogger<Rol> logger)
        {
            _context = context;
            _logger = logger;
        }

        ///<summary>
        ///Obtiene todos los roles almacenados en la base de datos.
        ///</summary>
        ///<returns> Lista de roles</returns>

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _context.Set<Rol>().ToListAsync();
        }

        ///<summary> Obtiene un rol específico por su identificador.

        public async Task<Rol?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Rol>().FindAsync(id);
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
        ///<param name="rol">Instancia del rol a crear</param>
        ///<returns>El rol creado</returns>

        public async Task<Rol> CreateAsync(Rol rol)
        {
            try
            {
                await _context.Set<Rol>().AddAsync(rol);
                await _context.SaveChangesAsync();
                return rol;
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
        ///<param name="rol">Objeto con la información actualizada</param>
        ///<returns>True si la operación fue exitosa, False en caso contrario.</returns>

        public async Task<bool> UpdateAsync(Rol rol)
        {
            try
            {
                _context.Set<Rol>().Update(rol);
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
                var rol = await _context.Set<Rol>().FindAsync(id);
                if (rol == null)
                    return false;

                rol.Active = false;
                _context.Entry(rol).Property(r => r.Active).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar (lógicamente) el rol con ID {id}");
                return false;
            }
        }



        public async Task<bool> RolAsync(int id, string name, string typeRol, bool active)
        {
            try
            {
                var rol = await _context.Set<Rol>().FindAsync(id);
                if (rol == null)
                    return false;

                rol.Name = name;
                rol.typeRol = typeRol;
                rol.Active = active;

                _context.Entry(rol).Property(r => r.Name).IsModified = true;
                _context.Entry(rol).Property(r => r.typeRol).IsModified = true;
                _context.Entry(rol).Property(r => r.Active).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al aplicar patch al rol con ID {id}");
                return false;
            }
        }





        public async Task<bool> PutRolAsync(int id, string name, string typeRol, bool active)
        {
            try
            {
                var rol = await _context.Rol.FindAsync(id);
                if (rol == null)
                    return false;

                rol.Name = name;
                rol.typeRol = typeRol;
                rol.Active = active;

                _context.Entry(rol).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar el rol con ID {id}");
                return false;
            }
        }























    }
}





        
    

