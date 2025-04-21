using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public  class LineThematic
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Active { get; set; } 
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}