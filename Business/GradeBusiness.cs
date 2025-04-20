using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exeptions;
using ValidationException = Utilities.Exeptions.ValidationException;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los grados en el sistema.
    /// </summary>
    public class GradeBusiness
    {
        private readonly GradeData _gradeData;
        private readonly ILogger<Grade> _logger;

        public GradeBusiness(GradeData gradeData, ILogger<Grade> logger)
        {
            _gradeData = gradeData;
            _logger = logger;
        }

        // Método para obtener todos los grados como DTOs
        public async Task<IEnumerable<GradeDTO>> GetAllGradesAsync()
        {
            try
            {
                var grades = await _gradeData.GetAllAsync();
               

                return MapToDTOList(grades);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los grados");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de grados", ex);
            }
        }

        // Método para obtener un grado por ID como DTO
        public async Task<GradeDTO> GetGradeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un grado con ID inválido: {GradeId}", id);
                throw new Utilities.Exeptions.ValidationException("id", "El ID del grado debe ser mayor que cero");
            }

            try
            {
                var grade = await _gradeData.GetByIdAsync(id);
                if (grade == null)
                {
                    _logger.LogInformation("No se encontró ningún grado con ID: {GradeId}", id);
                    throw new EntityNotFoundException("Grade", id);
                }



                return MapToDTO(grade);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado con ID: {GradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado con ID {id}", ex);
            }
        }

        // Método para crear un grado desde un DTO
        public async Task<GradeDTO> CreateGradeAsync(GradeDTO GradeDTO)
        {
            try
            {
                ValidateGrade(GradeDTO);

                var grade= MapToEntity(GradeDTO);

                var gradeCreate = await _gradeData.CreateAsync(grade);

                return MapToDTO(gradeCreate);







            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado: {GradeName}", GradeDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateGrade(GradeDTO GradeDTO)
        {
            if (GradeDTO == null)
            {
                throw new Utilities.Exeptions.ValidationException("El objeto grado no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(GradeDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado con Name vacío");
                throw new Utilities.Exeptions.ValidationException("Name", "El Name del grado es obligatorio");
            }

           
        }



        // Método PATCH para modificar parcialmente una grade
        public async Task<bool> GradeAsync(int id, string name, bool active)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException("name", "El nombre de la grade es obligatorio.");

            var result = await _gradeData.PatchGradeAsync(id, name, active);

            if (!result)
                throw new EntityNotFoundException("Grade", id);

            return true;
        }

        // Método PUT para modificar completamente una grade
        public async Task<bool> PutGradeAsync(int id, GradeDTO dto)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            if (dto == null)
                throw new ValidationException("Grade", "Datos de grade inválidos.");

            var result = await _gradeData.PutGradeAsync(id, dto.Name, dto.Active);

            if (!result)
                throw new EntityNotFoundException("Grade", id);

            return true;
        }

        // Método DELETE lógico
        public async Task<bool> DeleteGradeAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("id", "El ID debe ser mayor que cero.");

            var deleted = await _gradeData.DeleteAsync(id);

            if (!deleted)
                throw new EntityNotFoundException("Grade", id);

            return true;
        }

















        //  Metodo para mapear de entidad a DTO 

        private GradeDTO MapToDTO(Grade grade)
        {
            return new GradeDTO
            {
                Id = grade.Id,
                Name = grade.Name,
                Active = grade.Active
            };
        }

        // Metodo para mapear de DTO a entidad
        private Grade MapToEntity(GradeDTO gradeDTO)
        {
            return new Grade
            {
                Id = gradeDTO.Id,
                Name = gradeDTO.Name,
                Active = gradeDTO.Active
              
            };
        }


        // Método para mapear una lista de entidades a una lista de DTOs
        private IEnumerable<GradeDTO> MapToDTOList(IEnumerable<Grade> grades)
        {
           var gradeDTOs = new List<GradeDTO>();
            foreach (var grade in grades)
            {
                gradeDTOs.Add(MapToDTO(grade));
            }
            return gradeDTOs;
        }




    }
}