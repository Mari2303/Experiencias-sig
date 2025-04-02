
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

        public int GradeId { get; set; }
        public Grade Grade { get; set; }
        
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}