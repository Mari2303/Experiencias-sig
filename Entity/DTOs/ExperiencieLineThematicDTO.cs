using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs
{
 public  class ExperiencieLineThematicDTO
    {
        public int Id { get; set; }
       
        public int ExperiencieId { get; set; }
        public string ExperiencieName { get; set; } = string.Empty;
        public int LineThematicId { get; set; }
        public string LineThematicName { get; set; } = string.Empty;
    }
}
