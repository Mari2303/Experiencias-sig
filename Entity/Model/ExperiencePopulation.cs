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
        public string ExperiencieName { get; set; } = string.Empty;

        public int PopulationGradeId { get; set; }
        public string PopulationGradeName { get; set; } = string.Empty;

        public PopulationGrade PopulationGrade { get; set; }

        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}