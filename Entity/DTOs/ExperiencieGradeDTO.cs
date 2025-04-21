using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
public    class ExperiencieGradeDTO
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int ExperiencieId { get; set; }
        public string ExperiencieName { get; set; } = string.Empty;
    }
}