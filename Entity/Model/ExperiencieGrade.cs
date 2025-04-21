
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public class ExperiencieGrade
    {
        public int Id { get; set; }
                
        public int ExperiencieId { get; set; }
        public Experiencie Experiencie { get; set; }  
        public string ExperiencieName { get; set; } = string.Empty;

        public int GradeId { get; set; }
        public Grade Grade { get; set; }
        public string GradeName { get; set; } = string.Empty;

        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}