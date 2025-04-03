using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public    class ObjectiveDTO
    {
        public int Id { get; set; }
        public string ObjetiveDescription { get; set; }
        public string Innovation { get; set; }
        public string Results { get; set; }
        public string Sustainability { get; set; }
        public int ExperienceId { get; set; }
    }
}
