using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
public class Institucion
    {
        public int Id { get; set; }
    
        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; }  = string.Empty;

        public string EmailInstitution { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string Commune { get; set; } = string.Empty;
        
        public bool Active { get; set; }
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}