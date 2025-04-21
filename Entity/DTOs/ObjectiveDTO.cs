using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
public    class ObjectiveDTO
    {
        public int Id { get; set; }
        public string ObjetiveDescription { get; set; } = string.Empty;
        public string Innovation { get; set; } = string.Empty;
        public string Results { get; set; } = string.Empty;
        public string Sustainability { get; set; } = string.Empty;
        public int ExperiencieId { get; set; }
        public string ExperiencieName { get; set; } = string.Empty;
    }
}
