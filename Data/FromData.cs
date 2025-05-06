﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class FromData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FromData> _logger;

        public FromData(ApplicationDbContext context, ILogger<FromData> logger)
        {
            _context = context;
            _logger = logger;
        }


        /// <summary>
        /// Obtiene todos los forms almacenados en la base de datos
        /// </summary>
        /// <returns> Lista de forms </returns>
        public async Task<IEnumerable<From>> GetAllAsync()
        {
            return await _context.Set<From>()
                 .Where(f => f.Active)//Trae solo los activos
                 .ToListAsync();
        }

        public async Task<From> GetByidAsync(int id)
        {
            try
            {
                return await _context.Set<From>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener formulario con ID{id}");
                throw;
            }
        }

        public async Task<From> CreateAsync(From form)
        {
            try
            {
                await _context.Set<From>().AddAsync(form);
                await _context.SaveChangesAsync();
                return form;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el formulario {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(From form)
        {
            try
            {
                _context.Set<From>().Update(form);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el formulario {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var form = await _context.Set<From>().FindAsync(id);
                if (form == null)
                    return false;

                _context.Set<From>().Remove(form);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el formulario {ex.Message}");
                return false;
            }
        }

        ///<summary>
        /// Elimina logicamente un form (desactiva o activia )
        /// </summary>
        /// <param name="id">Id del form</param>
        /// <returns>True si la operacion fue exitosa</returns>
        public async Task<bool> SetActiveAsync(int id, bool active)
        {
            try
            {
                var form = await _context.Set<From>().FindAsync(id);
                if (form == null)
                    return false;

                form.Active = active; //Desactiva el fomr
                _context.Entry(form).Property(f => f.Active).IsModified = true;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al realizar eliminacion logica del form con ID {id}");
                return false;
            }
        }

        ///<summary>
        ///Modifica datos especificos de form
        ///</summary>
        ///<param name="id">Id del form</param>
        ///<returns> True si la actualizacion es verdadera</returns>
        public async Task<bool> PatchAsync(int id, string newDescription, string newName)
        {
            try
            {
                var form = await _context.Set<From>().FindAsync(id);
                if (form == null)
                    return false;


                form.Name = newName;
                form.Description = newDescription;


                _context.Entry(form).Property(f => f.Description).IsModified = true;




                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al modificar datos del form con su Id");
                return false;
            }

        }
    }
}
