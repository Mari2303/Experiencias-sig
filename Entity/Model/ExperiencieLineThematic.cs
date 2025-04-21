using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
 public  class ExperiencieLineThematic
    {
        public int Id { get; set; }
       
        public int  LineThematicId { get; set; }
        public LineThematic LineThematic { get; set; }
        public string LineThematicName { get; set; } = string.Empty;

        public  int ExperiencieId { get; set; }
        public Experiencie Experiencie { get; set; }
        public string ExperiencieName { get; set; } = string.Empty;
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}