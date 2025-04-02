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
    
        public string name { get; set; } 

        public string address { get; set; }

        public string phone { get; set; }

        public string emailInstitution { get; set; }

        public string department { get; set; }

        public string commune { get; set; }       
        public DateTime DeleteAt { get; set; }
        public DateTime CreateAt { get; set; }

    }
}