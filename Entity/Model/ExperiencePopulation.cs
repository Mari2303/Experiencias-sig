using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class ExperiencePopulation
    {
        public int Id { get; set; }
        public int ExperiencieId { get; set; }

        public Experiencie Experiencie { get; set; }
        
        public int PopulationGradeId { get; set; }

        public PopulationGrade PopulationGrade { get; set; }

        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}